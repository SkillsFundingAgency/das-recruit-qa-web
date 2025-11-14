using Microsoft.Extensions.Logging;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using SFA.DAS.Encoding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.Services.EmployerAccount;

public class EmployerAccountProvider(
    ILogger<EmployerAccountProvider> logger,
    IRecruitOuterApiClient outerApiClient,
    IEncodingService encodingService)
    : IEmployerAccountProvider
{
    public async Task<IEnumerable<AccountLegalEntity>> GetLegalEntitiesConnectedToAccountAsync(string hashedAccountId)
    {
        try
        {
            var accountId = encodingService.Decode(hashedAccountId, EncodingType.AccountId);
            var legalEntities =
                await outerApiClient.Get<GetAccountLegalEntitiesResponse>(
                    new GetAccountLegalEntitiesRequest(accountId));
                
            return legalEntities.AccountLegalEntities;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Failed to retrieve account information for account Id: {hashedAccountId}");
            throw;
        }
    }
}