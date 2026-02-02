using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Recruit.Vacancies.Client.Application.Configuration;
using Recruit.Vacancies.Client.Domain.Models;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Services;

[TestFixture]
public class TrainingProviderServiceTests
{
    [Test]
    public async Task GetProviderAsync_ShouldReturnEsfaTestTrainingProvider()
    {
        var loggerMock = new Mock<ILogger<TrainingProviderService>>();
        var outerApiClient = new Mock<IRecruitQaOuterApiVacancyClient>();

        var sut = new TrainingProviderService(loggerMock.Object, outerApiClient.Object);

        var provider = await sut.GetProviderAsync(EsfaTestTrainingProvider.Ukprn);

        provider.Ukprn.Should().Be(EsfaTestTrainingProvider.Ukprn);
        provider.Name.Should().Be(EsfaTestTrainingProvider.Name);
        provider.Address.Postcode.Should().Be("CV1 2WT");
    }

    [Test, MoqAutoData]
    public async Task GetProviderAsync_ShouldAttemptToFindTrainingProvider(Provider provider)
    {
        const long ukprn = 88888888;

        var loggerMock = new Mock<ILogger<TrainingProviderService>>();
        var outerApiClient = new Mock<IRecruitQaOuterApiVacancyClient>();
        
        var sut = new TrainingProviderService(loggerMock.Object, outerApiClient.Object);
        outerApiClient.Setup(x => x.GetProviderAsync(ukprn)).ReturnsAsync(provider);

        var result = await sut.GetProviderAsync(ukprn);

        result.Name.Should().Be(provider.Name);
        result.Ukprn.Should().Be(provider.Ukprn);
        result.Address.Should().NotBeNull();
        result.Address.AddressLine1.Should().Be(provider.Address.Address1);
        result.Address.AddressLine2.Should().Be(provider.Address.Address2);
        result.Address.AddressLine3.Should().Be(provider.Address.Address3);
        result.Address.AddressLine4.Should().Be(provider.Address.Address4);
        result.Address.Postcode.Should().Be(provider.Address.Postcode);
    }
}