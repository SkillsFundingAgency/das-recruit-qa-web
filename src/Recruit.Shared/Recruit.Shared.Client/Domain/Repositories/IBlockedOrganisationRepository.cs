using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Domain.Repositories;

public interface IBlockedOrganisationRepository
{
    Task CreateAsync(BlockedOrganisation organisation);
}