using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;

public interface ITrainingProviderService
{
    Task<Domain.Entities.TrainingProvider> GetProviderAsync(long ukprn);
}