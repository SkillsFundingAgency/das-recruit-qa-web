using AutoFixture.NUnit3;
using Recruit.Vacancies.Client.Infrastructure.User;
using Recruit.Vacancies.Client.Infrastructure.User.Requests;
using NUnit.Framework;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Vacancies.Client.Infrastructure.Client.User;

public class UserServiceTests
{
    [Test, MoqAutoData]
    public async Task When_Calling_UpsertUserAsync_The_Outer_Api_Is_Called(
        Recruit.Vacancies.Client.Domain.Entities.User user,
        [Frozen] Mock<IRecruitOuterApiClient> outerApiClient,
        UserService userService)
    {
        outerApiClient.Setup(x => x.Post(It.Is<PostUserRequest>(y => y.PostUrl.Contains(user.Id.ToString())), false));
        
        await userService.UpsertUserAsync(user);
        
        outerApiClient.Verify(x => x.Post(It.Is<PostUserRequest>(y => 
            y.PostUrl.Contains(user.Id.ToString())
            && ((UserDto)y.Data).IdamsUserId == user.IdamsUserId
            ), false),Times.Once());
        
    }
}