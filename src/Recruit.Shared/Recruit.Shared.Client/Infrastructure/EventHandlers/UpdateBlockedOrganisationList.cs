using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Events;
using Recruit.Vacancies.Client.Infrastructure.Services.Projections;
using MediatR;

namespace Recruit.Vacancies.Client.Infrastructure.EventHandlers;

public class UpdateBlockedOrganisationList : INotificationHandler<ProviderBlockedEvent>
{
    private readonly IBlockedOrganisationsProjectionService _projectionService;
    public UpdateBlockedOrganisationList(IBlockedOrganisationsProjectionService projectionService)
    {
        _projectionService = projectionService;
    }
    public Task Handle(ProviderBlockedEvent notification, CancellationToken cancellationToken)
    {
        return _projectionService.RebuildAllBlockedOrganisationsAsync();
    }
}