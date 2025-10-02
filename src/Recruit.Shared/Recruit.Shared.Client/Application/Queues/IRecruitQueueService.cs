using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Application.Queues;

public interface IRecruitQueueService
{
    Task AddMessageAsync<T>(T message);
}