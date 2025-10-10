using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Recruit.Qa.Web.Security;

public class UserAuthorizationService(IAuthorizationService authorizationService)
{
    public async Task<bool> IsTeamLeadAsync(ClaimsPrincipal user)
    {
        var authResult =
            await authorizationService.AuthorizeAsync(user, AuthorizationPolicyNames.TeamLeadUserPolicyName);

        return authResult.Succeeded;
    }
}