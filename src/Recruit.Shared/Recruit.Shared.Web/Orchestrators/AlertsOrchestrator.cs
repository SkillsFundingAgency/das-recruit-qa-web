using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Infrastructure.Client;

namespace Recruit.Shared.Web.Orchestrators;

public class AlertsOrchestrator
{
    private readonly IRecruitVacancyClient _client;
    private readonly ITimeProvider _timeProvider;

    private readonly IMessaging _messaging;

    public AlertsOrchestrator(IRecruitVacancyClient client, ITimeProvider timeProvider, IMessaging messaging)
    {
        _client = client;
        _timeProvider = timeProvider;
        _messaging = messaging;
    }

    public Task DismissAlert(VacancyUser user, AlertType alertType)
    {
        return _messaging.SendCommandAsync(new UpdateUserAlertCommand
        {
            IdamsUserId = user.UserId,
            AlertType = alertType,
            DismissedOn = _timeProvider.Now,
            DfEUserId = user.DfEUserId
        });
    }
}