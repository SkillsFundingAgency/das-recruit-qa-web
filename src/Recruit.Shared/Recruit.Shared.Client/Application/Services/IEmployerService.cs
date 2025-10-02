using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Services;

public interface IEmployerService
{
    Task<string> GetEmployerNameAsync(Vacancy vacancy);
    Task<string> GetEmployerDescriptionAsync(Vacancy vacancy);
}