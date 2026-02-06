using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;

namespace Recruit.Vacancies.Client.Infrastructure.OuterApi.Requests;

public class GetTrainingProgrammesRequest : IGetApiRequest
{
    public string GetUrl => $"trainingprogrammes";
}