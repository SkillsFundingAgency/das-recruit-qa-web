using System.Collections.Generic;

namespace Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.EditVacancyInfo;

public class EmployerEditVacancyInfo : QueryProjectionBase
{
    public EmployerEditVacancyInfo()  : base(QueryViewType.EditVacancyInfo.TypeName)
    {
    }

    public IEnumerable<LegalEntity> LegalEntities { get; set; }
}