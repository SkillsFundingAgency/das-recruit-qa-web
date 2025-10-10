using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Infrastructure.Services.Projections;

public class EditVacancyInfoProjectionService(
    ILogger<EditVacancyInfoProjectionService> logger,
    IQueryStoreWriter queryStoreWriter)
    : IEditVacancyInfoProjectionService
{
    public async Task UpdateEmployerVacancyDataAsync(string employerAccountId, IList<LegalEntity> legalEntities)
    {
        await queryStoreWriter.UpdateEmployerVacancyDataAsync(employerAccountId, legalEntities);

        logger.LogDebug($"Legal Entities inserted: {legalEntities.Count} for Employer: {employerAccountId}");
    }

    public async Task UpdateProviderVacancyDataAsync(long ukprn, IEnumerable<EmployerInfo> employers, bool hasAgreement)
    {
        await queryStoreWriter.UpdateProviderVacancyDataAsync(ukprn, employers, hasAgreement);

        logger.LogDebug($"Employers inserted: {employers.Count()} for Provider: {ukprn} has agreement:{hasAgreement}");
    }

}