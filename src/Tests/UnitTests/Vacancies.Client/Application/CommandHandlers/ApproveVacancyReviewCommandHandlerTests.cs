using System.Collections.Generic;
using System.Threading;
using AutoFixture.NUnit3;
using FluentValidation;
using FluentValidation.Results;
using Recruit.Vacancies.Client.Application.CommandHandlers;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Application.Communications;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Application.Validation.Fluent;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.StorageQueue;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NUnit.Framework;
using Recruit.Communication.Types;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Application.CommandHandlers;

public class ApproveVacancyReviewCommandHandlerTests
{
    private Guid _existingReviewId;
    private readonly Fixture _autoFixture = new ();
    private Mock<IVacancyReviewRepositoryRunner> _mockVacancyReviewRepository;
    private Mock<IVacancyReviewQuery> _mockVacancyReviewQuery;
    private Mock<IVacancyRepository> _mockVacancyRepository;
    private Mock<ITimeProvider> _mockTimeProvider;
    private Mock<IMessaging> _mockMessaging;
    private Mock<IRecruitQaOuterApiClient> _outerApiClient;
    private ApproveVacancyReviewCommandHandler _sut;
    private Mock<ICommunicationQueueService> _mockCommunicationQueueService;

    [SetUp]
    public void Setup()
    {
        _existingReviewId = Guid.NewGuid();
        _mockVacancyReviewRepository = new Mock<IVacancyReviewRepositoryRunner>();
        _mockVacancyRepository = new Mock<IVacancyRepository>();

        _mockMessaging = new Mock<IMessaging>();
        var mockValidator = new VacancyReviewValidator();

        _outerApiClient = new Mock<IRecruitQaOuterApiClient>();

        _mockTimeProvider = new Mock<ITimeProvider>();
        _mockTimeProvider.Setup(t => t.Now).Returns(DateTime.UtcNow);

        _mockCommunicationQueueService = new Mock<ICommunicationQueueService>();
        _mockVacancyReviewQuery = new Mock<IVacancyReviewQuery>();

        _sut = new ApproveVacancyReviewCommandHandler(Mock.Of<ILogger<ApproveVacancyReviewCommandHandler>>(), _mockVacancyReviewRepository.Object,
            _mockVacancyReviewQuery.Object, _mockVacancyRepository.Object, _mockMessaging.Object, mockValidator, 
            _mockTimeProvider.Object, _outerApiClient.Object, _mockCommunicationQueueService.Object);
    }

    [Test]
    [MoqInlineAutoData(ReviewStatus.Closed)]
    [MoqInlineAutoData(ReviewStatus.New)]
    [MoqInlineAutoData(ReviewStatus.PendingReview)]
    public async Task GivenApprovedVacancyReviewCommand_AndVacancyReviewIsNotUnderReview_ThenDoNotProcessApprovingReview(ReviewStatus reviewStatus)
    {
        _mockVacancyReviewQuery.Setup(x => x.GetAsync(_existingReviewId)).ReturnsAsync(new VacancyReview { Status = reviewStatus});

        var command = new ApproveVacancyReviewCommand(_existingReviewId, "comment", new List<ManualQaFieldIndicator>(), new List<Guid>(), new List<ManualQaFieldEditIndicator>());

        await _sut.Handle(command, CancellationToken.None);

        _mockVacancyReviewRepository.Verify(x => x.UpdateAsync(It.IsAny<VacancyReview>()), Times.Never);
        _mockMessaging.Verify(x => x.PublishEvent(It.IsAny<VacancyReviewApprovedEvent>()), Times.Never);
    }

    [Test]
    [MoqInlineAutoData(TransferReason.EmployerRevokedPermission, ClosureReason.TransferredByEmployer)]
    [MoqInlineAutoData(TransferReason.BlockedByQa, ClosureReason.TransferredByQa)]
    public async Task GivenApprovedVacancyReviewCommand_AndVacancyHasBeenTransferredSinceReviewWasCreated_ThenDoNotRaiseVacancyApprovedEventAndCloseVacancy(
        TransferReason transferReason,
        ClosureReason expectedClosureReason)
    {
        var transferInfo = new TransferInfo
        {
            Reason = transferReason
        };
            
        var existingVacancy = _autoFixture.Build<Vacancy>()
            .With(x => x.TransferInfo, transferInfo)
            .Create();

        _mockVacancyRepository.Setup(x => x.GetVacancyAsync(existingVacancy.VacancyReference.Value)).ReturnsAsync(existingVacancy);

        _mockVacancyReviewQuery.Setup(x => x.GetAsync(_existingReviewId)).ReturnsAsync(new VacancyReview
        {
            Id = _existingReviewId,
            CreatedDate = _mockTimeProvider.Object.Now.AddHours(-5),
            Status = ReviewStatus.UnderReview,
            VacancyReference = existingVacancy.VacancyReference!.Value,
            VacancySnapshot = new Vacancy()
        });

        var command = new ApproveVacancyReviewCommand(_existingReviewId, "comment", new List<ManualQaFieldIndicator>(), new List<Guid>(), new List<ManualQaFieldEditIndicator>());

        await _sut.Handle(command, CancellationToken.None);

        _mockVacancyReviewRepository.Verify(x => x.UpdateAsync(It.Is<VacancyReview>(r => r.Id == _existingReviewId)), Times.Once);
        _mockMessaging.Verify(x => x.PublishEvent(It.IsAny<VacancyReviewApprovedEvent>()), Times.Never);

        existingVacancy.Status.Should().Be(VacancyStatus.Closed);
        existingVacancy.ClosureReason.Should().Be(expectedClosureReason);
        _mockVacancyRepository.Verify(x => x.UpdateAsync(existingVacancy), Times.Once);
        _mockCommunicationQueueService.Verify(c => c.AddMessageAsync(It.Is<CommunicationRequest>(r => r.RequestType == CommunicationConstants.RequestType.ProviderBlockedEmployerNotificationForLiveVacancies)));
    }
    
    [Test, MoqAutoData]
    public async Task The_Publish_Vacancy_Call_Is_Made(
        Guid vacancyReviewId,
        Vacancy existingVacancy,
        [Frozen] Mock<IVacancyReviewQuery> vacancyReviewQuery,
        [Frozen] Mock<IVacancyRepository> vacancyRepository,
        [Frozen] Mock<IValidator<VacancyReview>> vacancyReviewValidator,
        [Frozen] Mock<IMessageSession> messageSession,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        [Greedy] ApproveVacancyReviewCommandHandler sut)
    {
        // arrange
        existingVacancy.TransferInfo = null;
        vacancyReviewQuery.Setup(x => x.GetAsync(vacancyReviewId)).ReturnsAsync(new VacancyReview
        {
            Id = vacancyReviewId,
            CreatedDate = _mockTimeProvider.Object.Now.AddHours(-5),
            Status = ReviewStatus.UnderReview,
            VacancyReference = existingVacancy.VacancyReference!.Value,
            VacancySnapshot = new Vacancy()
        });
        vacancyRepository.Setup(x => x.GetVacancyAsync(existingVacancy.VacancyReference.Value)).ReturnsAsync(existingVacancy);
        vacancyReviewValidator.Setup(x => x.Validate(It.IsAny<VacancyReview>())).Returns(new ValidationResult());

        var command = new ApproveVacancyReviewCommand(vacancyReviewId, "comment", [], [], []);

        // act
        await sut.Handle(command, CancellationToken.None);
        
        // assert
        outerApiClient.Verify(x => x.Post(It.Is<PostPublishVacancyRequest>(r => r.VacancyId == existingVacancy.Id), true), Times.Once);
    }
}