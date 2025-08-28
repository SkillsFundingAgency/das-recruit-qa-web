using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Application.Services;

public interface IGenerateVacancyNumbers
{
    Task<long> GenerateAsync();
}