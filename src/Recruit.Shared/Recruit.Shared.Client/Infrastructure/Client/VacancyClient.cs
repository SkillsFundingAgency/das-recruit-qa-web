using Esfa.Recruit.Vacancies.Client.Application.Commands;
using Esfa.Recruit.Vacancies.Client.Domain.Messaging;
using Esfa.Recruit.Vacancies.Client.Domain.Repositories;
using Esfa.Recruit.Vacancies.Client.Infrastructure.OuterApi.Responses;
using Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Services.EmployerAccount;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.Client;

public partial class VacancyClient(
    IQueryStoreReader reader,
    IMessaging messaging,
    IEmployerAccountProvider employerAccountProvider,
    IVacancyReviewQuery vacancyReviewQuery,
    IUserRepository userRepository)
    : IRecruitVacancyClient, IEmployerVacancyClient
{
    public Task<IEnumerable<LegalEntity>> GetEmployerLegalEntitiesAsync(string employerAccountId)
    {
        return employerAccountProvider.GetEmployerLegalEntitiesAsync(employerAccountId);
    }

    public Task SetupEmployerAsync(string employerAccountId)
    {
        var command = new SetupEmployerCommand
        {
            EmployerAccountId = employerAccountId
        };

        return messaging.SendCommandAsync(command);
    }

    public Task<EmployerEditVacancyInfo> GetEditVacancyInfoAsync(string employerAccountId)
    {
        return reader.GetEmployerVacancyDataAsync(employerAccountId);
    }

    public Task<GetUserAccountsResponse> GetEmployerIdentifiersAsync(string userId, string email)
    {
        return employerAccountProvider.GetEmployerIdentifiersAsync(userId, email);
    }

    public Task<Domain.Entities.VacancyReview> GetCurrentReferredVacancyReviewAsync(long vacancyReference)
    {
        return vacancyReviewQuery.GetCurrentReferredVacancyReviewAsync(vacancyReference);
    }

    public Task<Domain.Entities.User> GetUsersDetailsAsync(string userId)
    {
        return userRepository.GetAsync(userId);
    }
        
    public Task<Domain.Entities.User> GetUsersDetailsByDfEUserId(string dfeUserId)
    {
        return userRepository.GetByDfEUserId(dfeUserId);
    }
}