using System.ComponentModel.DataAnnotations;

namespace Recruit.Shared.Web.ViewModels.Validations;

public class TypeOfIntegerAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        return (int.TryParse((string)value, out var i));
    }
}