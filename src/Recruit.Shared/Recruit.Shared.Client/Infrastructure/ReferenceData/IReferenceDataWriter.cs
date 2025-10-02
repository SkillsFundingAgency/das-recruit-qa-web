using System.Threading.Tasks;

namespace Recruit.Vacancies.Client.Infrastructure.ReferenceData;

public interface IReferenceDataWriter
{
    Task UpsertReferenceData<T>(T referenceData) where T : class, IReferenceDataItem;
}