using AutoFixture.NUnit3;
using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Provider;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.VacancySummary;
using System.Collections.Generic;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.QueryStore;

public class WhenUpdatingProviderDashboard
{
    [Test, MoqAutoData]
    public async Task Then_If_Apprenticeship_Then_Updates_Provider_Apprentice_Dashboard(
        long ukprn,
        ProviderDashboard providerDashboard,
        List<VacancySummary> vacancySummaries,
        List<ProviderDashboardTransferredVacancy> providerDashboardTransferredVacancies,
        [Frozen] Mock<IQueryStore> queryStore,
        QueryStoreClient client)
    {
        await client.UpdateProviderDashboardAsync(ukprn, vacancySummaries, providerDashboardTransferredVacancies);

        queryStore
            .Verify(x =>
                x.UpsertAsync(It.Is<ProviderDashboard>(c =>
                    c.Id.Equals(QueryViewType.ProviderDashboard.GetIdValue(ukprn))
                    && c.Vacancies.Equals(vacancySummaries)
                    && c.TransferredVacancies.Equals(providerDashboardTransferredVacancies)
                )), Times.Once);
    }
        
}