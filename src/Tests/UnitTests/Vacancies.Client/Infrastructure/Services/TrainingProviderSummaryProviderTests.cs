using Recruit.Vacancies.Client.Application.Configuration;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProviderSummaryProvider;
using Xunit;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Services;

public class TrainingProviderSummaryProviderTests
{
    [Fact]
    public async Task GetAsync_ShouldReturnEsfaTestProviderForUkrpn()
    {
        var providerClientMock = new Mock<ITrainingProviderService>();

        var sut = new TrainingProviderSummaryProvider(providerClientMock.Object);

        var provider = await sut.GetAsync(EsfaTestTrainingProvider.Ukprn);

        provider.Ukprn.Should().Be(EsfaTestTrainingProvider.Ukprn);
        provider.ProviderName.Should().Be(EsfaTestTrainingProvider.Name);
        providerClientMock.Verify(c => c.FindAllAsync(), Times.Never);
    }

    [Fact]
    public async Task GetAsync_ShouldAttemptToFindTrainingProvider()
    {
        var ukprn = 88888888;
        var providerClientMock = new Mock<ITrainingProviderService>();
        providerClientMock.Setup(p => p.GetProviderAsync(ukprn))
            .ReturnsAsync(new TrainingProvider
            {
                Ukprn = 88888888, 
                Name = "provider 1"
            });

        var sut = new TrainingProviderSummaryProvider(providerClientMock.Object);

        var provider = await sut.GetAsync(88888888);

        provider.Ukprn.Should().Be(88888888);
        provider.ProviderName.Should().Be("provider 1");
    }
}