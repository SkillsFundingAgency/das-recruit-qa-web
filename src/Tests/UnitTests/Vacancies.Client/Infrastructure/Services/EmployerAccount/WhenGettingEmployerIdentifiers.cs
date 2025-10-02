using AutoFixture.NUnit3;
using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using Recruit.Vacancies.Client.Infrastructure.Services.EmployerAccount;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Services.EmployerAccount;

public class WhenGettingEmployerIdentifiers
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_MAde_And_Data_returned(
        string email,
        string userId,
        GetUserAccountsResponse response,
        [Frozen] Mock<IRecruitOuterApiClient> outerApiClient,
        EmployerAccountProvider provider)
    {
        var request = new GetUserAccountsRequest(userId, email);
        outerApiClient
            .Setup(x => x.Get<GetUserAccountsResponse>(
                It.Is<GetUserAccountsRequest>(c => c.GetUrl.Equals(request.GetUrl)))).ReturnsAsync(response);

        var actual = await provider.GetEmployerIdentifiersAsync(userId, email);

        actual.Should().BeEquivalentTo(response);
    }
}