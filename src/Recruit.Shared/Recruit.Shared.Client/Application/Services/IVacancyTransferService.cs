using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Services;

public interface IVacancyTransferService
{
    Task TransferVacancyToLegalEntityAsync(Vacancy vacancy, VacancyUser initiatingUser, TransferReason transferReason);
}