using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Recruit.Vacancies.Client.Application.Cache;
using Recruit.Vacancies.Client.Application.Configuration;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.TrainingProviders;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;
using System.Collections.Generic;
using TrainingProvider = Recruit.Vacancies.Client.Infrastructure.ReferenceData.TrainingProviders.TrainingProvider;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Services;

[TestFixture]
public class TrainingProviderServiceTests
{
    [Test]
    public async Task GetProviderAsync_ShouldReturnEsfaTestTrainingProvider()
    {
        var loggerMock = new Mock<ILogger<TrainingProviderService>>();
        var referenceDataReader = new Mock<IReferenceDataReader>();
        var cache = new Mock<ICache>();
        var timeProvider = new Mock<ITimeProvider>();
        var outerApiClient = new Mock<IRecruitOuterApiClient>();

        var sut = new TrainingProviderService(loggerMock.Object, referenceDataReader.Object, cache.Object, timeProvider.Object, outerApiClient.Object);

        var provider = await sut.GetProviderAsync(EsfaTestTrainingProvider.Ukprn);

        provider.Ukprn.Should().Be(EsfaTestTrainingProvider.Ukprn);
        provider.Name.Should().Be(EsfaTestTrainingProvider.Name);
        provider.Address.Postcode.Should().Be("CV1 2WT");

        referenceDataReader.Verify(p => p.GetReferenceData<TrainingProviders>(), Times.Never);
    }

    [Test]
    public async Task GetProviderAsync_ShouldAttemptToFindTrainingProvider()
    {
        const long ukprn = 88888888;

        var loggerMock = new Mock<ILogger<TrainingProviderService>>();
        var referenceDataReader = new Mock<IReferenceDataReader>();
        var cache = new Mock<ICache>();
        var timeProvider = new Mock<ITimeProvider>();
        var outerApiClient = new Mock<IRecruitOuterApiClient>();
        var trainingProvider = new TrainingProvider
        {
            Ukprn = ukprn,
            Name = "name",
            Address = new TrainingProviderAddress
            {
                AddressLine1 = "address line 1",
                AddressLine2  = "address line 2",
                AddressLine3 = "address line 3",
                AddressLine4 = "address line 4",
                Postcode = "post code"
            }
        };
        var providers = new TrainingProviders
        {
            Data = new List<TrainingProvider>
            {
                trainingProvider
            }
        };
        cache.Setup(x => x.CacheAsideAsync(CacheKeys.TrainingProviders, It.IsAny<DateTime>(),
                It.IsAny<Func<Task<TrainingProviders>>>() ))
            .ReturnsAsync(providers);

        var sut = new TrainingProviderService(loggerMock.Object, referenceDataReader.Object, cache.Object, timeProvider.Object, outerApiClient.Object);

        var provider = await sut.GetProviderAsync(ukprn);

            
        provider.Name.Should().Be("name");
        provider.Ukprn.Should().Be(ukprn);
        provider.Address.AddressLine1.Should().Be("address line 1");
        provider.Address.AddressLine2.Should().Be("address line 2");
        provider.Address.AddressLine3.Should().Be("address line 3");
        provider.Address.AddressLine4.Should().Be("address line 4");
        provider.Address.Postcode.Should().Be("post code");
    }
}