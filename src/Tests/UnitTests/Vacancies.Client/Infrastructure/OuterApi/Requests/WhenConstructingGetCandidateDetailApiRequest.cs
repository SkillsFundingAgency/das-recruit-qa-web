using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using NUnit.Framework;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class WhenConstructingGetCandidateDetailApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built(Guid candidateId)
    {
        var actual = new GetCandidateDetailApiRequest(candidateId);
        actual.GetUrl.Should().Be($"candidates/{candidateId}");
    }
}