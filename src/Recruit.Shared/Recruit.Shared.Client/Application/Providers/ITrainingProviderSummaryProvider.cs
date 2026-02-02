using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Providers;

public interface ITrainingProviderSummaryProvider
{
    Task<TrainingProviderSummary> GetAsync(long ukprn);
}