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

public class UpdateDraftVacancyCommandHandlerTests
{

    [Test, MoqAutoData]
    public async Task GivenUpdateDraftVacancyCommand_ThenUpdatesVacancyWithTimestampAndUser(
        [Frozen] Mock<IMessaging> messaging,
        [Frozen] Mock<IVacancyRepository> vacancyRepository,
        UpdateDraftVacancyCommandHandler handler)
    {
        var vacancyId = Guid.NewGuid();
        var user = new VacancyUser { Email = "qa@test.com", DfEUserId = "user-1" };
        var vacancy = new VacancyQaFieldUpdate { Id = vacancyId, Status = nameof(VacancyStatus.Submitted)};
        var command = new UpdateDraftVacancyCommand { Vacancy = vacancy, User = user, EmployerAccountId = "ABC123" };

        await handler.Handle(command, CancellationToken.None);

        vacancyRepository.Verify(x => x.UpdateVacancyFromQaEdits(vacancy), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task GivenUpdateDraftVacancyCommand_ThenPublishesDraftVacancyUpdatedEvent(
        [Frozen] Mock<IMessaging> messaging,
        [Frozen] Mock<IVacancyRepository> vacancyRepository,
        UpdateDraftVacancyCommandHandler handler)
    {
        var vacancyId = Guid.NewGuid();
        var employerAccountId = "EMP456";
        var vacancy = new VacancyQaFieldUpdate { Id = vacancyId, Status = nameof(VacancyStatus.Submitted) };
        var command = new UpdateDraftVacancyCommand
        {
            Vacancy = vacancy,
            User = new VacancyUser { Email = "qa@test.com" },
            EmployerAccountId = employerAccountId
        };

        await handler.Handle(command, CancellationToken.None);

        messaging.Verify(x => x.PublishEvent(It.Is<DraftVacancyUpdatedEvent>(e =>
            e.VacancyId == vacancyId &&
            e.EmployerAccountId == employerAccountId)), Times.Once);
    }
}
