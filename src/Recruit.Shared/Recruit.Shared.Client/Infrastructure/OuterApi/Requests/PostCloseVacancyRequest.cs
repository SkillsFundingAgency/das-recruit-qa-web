using System;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class PostCloseVacancyRequest(Guid id, CloseVacancyRequest data) : IPostApiRequest
{
    public string PostUrl => $"vacancies/close/{id}";
    public object Data { get; set; } = data;
}

public class CloseVacancyRequest
{
    public string ClosureReason { get; set; }
}