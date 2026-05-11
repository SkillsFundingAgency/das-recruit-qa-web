using System.Threading;
using AutoFixture.NUnit4;
using NUnit.Framework;
using Recruit.Vacancies.Client.Application.CommandHandlers;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Repositories;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Application.CommandHandlers;

public class PublishVacancyCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task GivenApprovedVacancy_ThenPublishesVacancyAndFiresEvent(
        [Frozen] Mock<IVacancyRepository> vacancyRepository,
        [Frozen] Mock<IMessaging> messaging,
        PublishVacancyCommandHandler handler)
    {
        var vacancyId = Guid.NewGuid();
        var vacancy = new Vacancy { Id = vacancyId, Status = VacancyStatus.Approved };
        vacancyRepository.Setup(x => x.GetVacancyAsync(vacancyId)).ReturnsAsync(vacancy);

        var command = new PublishVacancyCommand { VacancyId = vacancyId };
        await handler.Handle(command, CancellationToken.None);

        vacancyRepository.Verify(x => x.PublishVacancy(vacancyId), Times.Once);
        messaging.Verify(x => x.PublishEvent(It.Is<VacancyPublishedEvent>(e => e.VacancyId == vacancyId)), Times.Once);
    }

    [Test]
    [MoqInlineAutoData(VacancyStatus.Live)]
    [MoqInlineAutoData(VacancyStatus.Draft)]
    [MoqInlineAutoData(VacancyStatus.Closed)]
    public async Task GivenVacancyNotEligibleToGoLive_ThenDoesNotPublish(
        VacancyStatus status,
        [Frozen] Mock<IVacancyRepository> vacancyRepository,
        [Frozen] Mock<IMessaging> messaging,
        PublishVacancyCommandHandler handler)
    {
        var vacancyId = Guid.NewGuid();
        var vacancy = new Vacancy { Id = vacancyId, Status = status };
        vacancyRepository.Setup(x => x.GetVacancyAsync(vacancyId)).ReturnsAsync(vacancy);

        var command = new PublishVacancyCommand { VacancyId = vacancyId };
        await handler.Handle(command, CancellationToken.None);

        vacancyRepository.Verify(x => x.PublishVacancy(It.IsAny<Guid>()), Times.Never);
        messaging.Verify(x => x.PublishEvent(It.IsAny<VacancyPublishedEvent>()), Times.Never);
    }
}
