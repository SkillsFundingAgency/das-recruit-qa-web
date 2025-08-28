using System;
using Recruit.Vacancies.Client.Domain.Entities;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.Provider;

public class ProviderDashboardTransferredVacancy
{
    public string LegalEntityName { get; set; }
    public DateTime TransferredDate { get; set; }
    public TransferReason Reason { get; set; }
}