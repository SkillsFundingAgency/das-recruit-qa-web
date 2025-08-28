using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.StorageQueue;

public interface ICommunicationQueueService
{
    Task AddMessageAsync<T>(T message);
}