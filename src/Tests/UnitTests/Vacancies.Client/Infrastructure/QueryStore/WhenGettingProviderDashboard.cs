using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Provider;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.QueryStore;

public class WhenGettingProviderDashboard
{
    [Test, MoqAutoData]
    public async Task Then_If_Apprenticeship_Then_Gets_Provider_Apprentice_Dashboard(
        long ukprn,
        ProviderDashboard providerDashboard,
        [Frozen] Mock<IQueryStore> queryStore,
        QueryStoreClient client)
    {
        queryStore
            .Setup(x => x.GetAsync<ProviderDashboard>(
                QueryViewType.ProviderDashboard.TypeName, 
                QueryViewType.ProviderDashboard.GetIdValue(ukprn))).ReturnsAsync(providerDashboard);

        var dashboard = await client.GetProviderDashboardAsync(ukprn);
            
        dashboard.Should().BeEquivalentTo(providerDashboard);
    }
}