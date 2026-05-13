using System;
using System.Threading;
using AutoFixture.NUnit4;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Recruit.Vacancies.Client.Application.CommandHandlers;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Repositories;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Application.CommandHandlers;

public class CloseVacancyCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task GivenLiveVacancy_ThenClosesVacancyAndPublishesEvent(
        [Frozen] Mock<IVacancyRepository> vacancyRepository,
        [Frozen] Mock<IMessaging> messaging,
        CloseVacancyCommandHandler handler)
    {
        var vacancyId = Guid.NewGuid();
        var user = new VacancyUser { Email = "qa@test.com", DfEUserId = "user-1" };
        var vacancy = new Vacancy { Id = vacancyId, VacancyReference = 1000000001L, Status = VacancyStatus.Live };
        vacancyRepository.Setup(x => x.GetVacancyAsync(vacancyId)).ReturnsAsync(vacancy);
        
        var command = new CloseVacancyCommand(vacancyId, user, ClosureReason.Manual);
        await handler.Handle(command, CancellationToken.None);

        vacancyRepository.Verify(x => x.CloseVacancy(vacancyId, ClosureReason.Manual), Times.Once);
        messaging.Verify(x => x.PublishEvent(It.Is<VacancyClosedEvent>(e => e.VacancyId == vacancyId && e.VacancyReference == 1000000001L)), Times.Once);
    }

    [Test]
    [MoqInlineAutoData(VacancyStatus.Closed)]
    [MoqInlineAutoData(VacancyStatus.Draft)]
    public async Task GivenNonLiveVacancy_ThenStillCallsCloseAndPublishesEvent(
        VacancyStatus status,
        [Frozen] Mock<IVacancyRepository> vacancyRepository,
        [Frozen] Mock<IMessaging> messaging,
        CloseVacancyCommandHandler handler)
    {
        var vacancyId = Guid.NewGuid();
        var user = new VacancyUser { Email = "qa@test.com", DfEUserId = "user-1" };
        var vacancy = new Vacancy { Id = vacancyId, VacancyReference = 1000000002L, Status = status };
        vacancyRepository.Setup(x => x.GetVacancyAsync(vacancyId)).ReturnsAsync(vacancy);
        
        var command = new CloseVacancyCommand(vacancyId, user, ClosureReason.Manual);
        await handler.Handle(command, CancellationToken.None);

        vacancyRepository.Verify(x => x.CloseVacancy(vacancyId, ClosureReason.Manual), Times.Once);
        messaging.Verify(x => x.PublishEvent(It.IsAny<VacancyClosedEvent>()), Times.Once);
    }
}
