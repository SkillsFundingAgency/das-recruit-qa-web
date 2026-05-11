using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class PostUpdateVacancyRequest(VacancyQaFieldUpdate data) : IPostApiRequest
{
    public string PostUrl => $"vacancies/update-from-qa/{data.Id}";
    public object Data { get; set; } = data;
}