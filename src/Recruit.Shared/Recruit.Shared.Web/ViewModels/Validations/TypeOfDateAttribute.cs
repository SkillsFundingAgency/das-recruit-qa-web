using System.ComponentModel.DataAnnotations;
using Recruit.Shared.Web.Extensions;

namespace Recruit.Shared.Web.ViewModels.Validations;

public class TypeOfDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        return ((string) value).AsDateTimeUk() != null;
    }
}