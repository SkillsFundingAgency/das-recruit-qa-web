using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Services.PasAccount;

public interface IPasAccountProvider
{
    Task<bool> HasAgreementAsync(long ukprn);
}