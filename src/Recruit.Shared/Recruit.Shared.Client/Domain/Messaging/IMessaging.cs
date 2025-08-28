using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Domain.Messaging;

public interface IMessaging
{
    Task<bool> SendStatusCommandAsync(ICommand command);
    Task SendCommandAsync(ICommand command);
    Task PublishEvent(IEvent @event);
}