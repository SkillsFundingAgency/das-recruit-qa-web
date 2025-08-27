using System.Threading.Tasks;

namespace Recruit.Shared.Web.Services;

public interface ITrainingProviderAgreementService
{
    Task<bool> HasAgreementAsync(long ukprn);
}