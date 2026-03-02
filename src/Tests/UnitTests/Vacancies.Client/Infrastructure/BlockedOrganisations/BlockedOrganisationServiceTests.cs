using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Requests;
using Recruit.Vacancies.Client.Infrastructure.BlockedOrganisations.Responses;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.BlockedOrganisations;

public class BlockedOrganisationServiceTests
{
    [Test, MoqAutoData]
    public async Task When_Calling_CreateAsync_The_Outer_Api_Is_Called(
        BlockedOrganisation organisation,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        BlockedOrganisationService service)
    {
        await service.CreateAsync(organisation);

        outerApiClient.Verify(x => x.Post(It.Is<PostBlockedOrganisationRequest>(y =>
            y.PostUrl == "blockedorganisations" &&
            ((BlockedOrganisationDto)y.Data).OrganisationId == organisation.OrganisationId
        ), true), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task When_Calling_GetByOrganisationIdAsync_The_Outer_Api_Is_Called_And_Mapped_Response_Returned(
        string organisationId,
        BlockedOrganisationDto apiResponse,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        BlockedOrganisationService service)
    {
        outerApiClient.Setup(x => x.Get<BlockedOrganisationDto>(It.Is<GetBlockedOrganisationRequest>(y =>
                y.GetUrl == $"blockedorganisations/{organisationId}")))
            .ReturnsAsync(apiResponse);

        var result = await service.GetByOrganisationIdAsync(organisationId);

        result.Should().BeEquivalentTo((BlockedOrganisation)apiResponse);
    }

    [Test, MoqAutoData]
    public async Task When_Calling_GetByOrganisationIdAsync_Returns_Null_If_Api_Returns_Null(
        string organisationId,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        BlockedOrganisationService service)
    {
        outerApiClient.Setup(x => x.Get<BlockedOrganisationDto>(It.IsAny<GetBlockedOrganisationRequest>()))
            .ReturnsAsync((BlockedOrganisationDto)null);

        var result = await service.GetByOrganisationIdAsync(organisationId);

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task When_Calling_GetAllBlockedProvidersAsync_The_Outer_Api_Is_Called_And_Mapped_Response_Returned(
        List<BlockedOrganisationSummaryDto> apiResponse,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        BlockedOrganisationService service)
    {
        outerApiClient.Setup(x => x.Get<List<BlockedOrganisationSummaryDto>>(It.Is<GetBlockedProvidersRequest>(y =>
                y.GetUrl == "blockedorganisations/providers")))
            .ReturnsAsync(apiResponse);

        var result = await service.GetAllBlockedProvidersAsync();

        result.Should().BeEquivalentTo(apiResponse.Select(s => (BlockedOrganisationSummary)s).ToList());
    }

    [Test, MoqAutoData]
    public async Task When_Calling_GetAllBlockedEmployersAsync_The_Outer_Api_Is_Called_And_Mapped_Response_Returned(
        List<BlockedOrganisationSummaryDto> apiResponse,
        [Frozen] Mock<IRecruitQaOuterApiClient> outerApiClient,
        BlockedOrganisationService service)
    {
        outerApiClient.Setup(x => x.Get<List<BlockedOrganisationSummaryDto>>(It.Is<GetBlockedEmployersRequest>(y =>
                y.GetUrl == "blockedorganisations/employers")))
            .ReturnsAsync(apiResponse);

        var result = await service.GetAllBlockedEmployersAsync();

        result.Should().BeEquivalentTo(apiResponse.Select(s => (BlockedOrganisationSummary)s).ToList());
    }
}
