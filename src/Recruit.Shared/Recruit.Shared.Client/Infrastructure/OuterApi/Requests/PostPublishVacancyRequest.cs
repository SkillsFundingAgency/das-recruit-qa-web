using System;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class PostPublishVacancyRequest(Guid id) : IPostApiRequest
{
    public string PostUrl => $"vacancies/publish/{id}";
    public object Data { get; set; }
}