using Esfa.Recruit.Vacancies.Client.Application.Configuration;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Services.PasAccount;
using Microsoft.Extensions.Options;
using Xunit;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Services;

public class PasAccountProviderTests
{
    [Fact]
    public async Task HasAgreementAsync_EsfaTestTrainingProviderShouldHaveAgreement()
    {
        var config = new Mock<IOptions<PasAccountApiConfiguration>>();
        var sut = new PasAccountProvider(config.Object);

        var result = await sut.HasAgreementAsync(EsfaTestTrainingProvider.Ukprn);

        result.Should().BeTrue();
    }
}