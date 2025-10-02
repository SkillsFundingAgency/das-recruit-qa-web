using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Infrastructure.User;
using MediatR;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Application.CommandHandlers;

public class UpdateUserAlertCommandHandler : IRequestHandler<UpdateUserAlertCommand, Unit>
{
    private readonly IRecruitVacancyClient _client;
    private readonly IUserRepositoryRunner _userRepository;
    public UpdateUserAlertCommandHandler(IRecruitVacancyClient client, IUserRepositoryRunner userRepository)
    {
        _userRepository = userRepository;
        _client = client;
    }

    public async Task<Unit> Handle(UpdateUserAlertCommand message, CancellationToken cancellationToken)
    {
        var user = string.IsNullOrEmpty(message.DfEUserId) 
            ? await _client.GetUsersDetailsAsync(message.IdamsUserId) 
            : await _client.GetUsersDetailsByDfEUserId(message.DfEUserId);

        switch (message.AlertType)
        {
            case AlertType.TransferredVacanciesEmployerRevokedPermission:
                user.TransferredVacanciesEmployerRevokedPermissionAlertDismissedOn = message.DismissedOn;
                break;
            case AlertType.ClosedVacanciesBlockedProvider:
                user.ClosedVacanciesBlockedProviderAlertDismissedOn = message.DismissedOn;
                break;
            case AlertType.TransferredVacanciesBlockedProvider:
                user.TransferredVacanciesBlockedProviderAlertDismissedOn = message.DismissedOn;
                break;
            case AlertType.ClosedVacanciesWithdrawnByQa:
                user.ClosedVacanciesWithdrawnByQaAlertDismissedOn = message.DismissedOn;
                break;
            default:
                throw new InvalidEnumArgumentException($"Cannot handle this alert dismissal {message.AlertType}");
        }

        await _userRepository.UpsertUserAsync(user);
            
        return Unit.Value;
    }
}