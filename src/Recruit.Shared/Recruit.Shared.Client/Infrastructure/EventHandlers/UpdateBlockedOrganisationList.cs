using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Infrastructure.Services.Projections;
using MediatR;

namespace Recruit.Vacancies.Client.Infrastructure.EventHandlers;

public class UpdateBlockedOrganisationList(IBlockedOrganisationsProjectionService projectionService)
    : INotificationHandler<ProviderBlockedEvent>
{
    public Task Handle(ProviderBlockedEvent notification, CancellationToken cancellationToken)
    {
        return projectionService.RebuildAllBlockedOrganisationsAsync();
    }
}