using System;
using Recruit.Vacancies.Client.Infrastructure.OuterApi;

namespace Recruit.Vacancies.Client.Infrastructure.User.Requests;

public class PostUserRequest(Guid id, UserDto user) : IPostApiRequest
{
    public string PostUrl => $"users/{id}";
    public object Data { get; set; } = user;
}