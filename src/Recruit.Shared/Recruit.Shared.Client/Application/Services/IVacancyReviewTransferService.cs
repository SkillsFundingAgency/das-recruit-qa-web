using System.Threading.Tasks;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Application.Services;

public interface IVacancyReviewTransferService
{
    Task CloseVacancyReview(long vacancyReference, TransferReason transferReason);
}