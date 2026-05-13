using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Recruit.Vacancies.Client.Application.CommandHandlers;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Application.Validation.Fluent;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Application.CommandHandlers;

public class ApproveVacancyReviewCommandHandlerTests
{
    private Guid _existingReviewId;
    private Mock<IVacancyReviewRepository> _mockVacancyReviewRepository;
    private Mock<IVacancyReviewQuery> _mockVacancyReviewQuery;
    private Mock<IVacancyRepository> _mockVacancyRepository;
    private Mock<ITimeProvider> _mockTimeProvider;
    private Mock<IMessaging> _mockMessaging;
    private ApproveVacancyReviewCommandHandler _sut;

    [SetUp]
    public void Setup()
    {
        _existingReviewId = Guid.NewGuid();
        _mockVacancyReviewRepository = new Mock<IVacancyReviewRepository>();
        _mockVacancyRepository = new Mock<IVacancyRepository>();

        _mockMessaging = new Mock<IMessaging>();
        var mockValidator = new VacancyReviewValidator();

        _mockTimeProvider = new Mock<ITimeProvider>();
        _mockTimeProvider.Setup(t => t.Now).Returns(DateTime.UtcNow);

        _mockVacancyReviewQuery = new Mock<IVacancyReviewQuery>();

        _sut = new ApproveVacancyReviewCommandHandler(Mock.Of<ILogger<ApproveVacancyReviewCommandHandler>>(), _mockVacancyReviewRepository.Object,
            _mockVacancyReviewQuery.Object, _mockVacancyRepository.Object, _mockMessaging.Object, mockValidator, 
            _mockTimeProvider.Object);
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
}