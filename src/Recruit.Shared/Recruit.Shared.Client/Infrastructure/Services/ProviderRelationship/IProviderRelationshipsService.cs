using Esfa.Recruit.Vacancies.Client.Domain.Models;
using System.Threading.Tasks;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.Services.ProviderRelationship;

public interface IProviderRelationshipsService
{
    Task<bool> HasProviderGotEmployersPermissionAsync(long ukprn, string accountPublicHashedId, string accountLegalEntityPublicHashedId, OperationType operationType);
}