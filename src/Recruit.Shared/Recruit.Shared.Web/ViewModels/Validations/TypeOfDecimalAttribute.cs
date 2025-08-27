using System.ComponentModel.DataAnnotations;
using Recruit.Shared.Web.Extensions;

namespace Recruit.Shared.Web.ViewModels.Validations;

public class TypeOfDecimalAttribute : ValidationAttribute
{
    private readonly int _numberOfDecimalPlaces;

    public TypeOfDecimalAttribute(int numberOfDecimalPlaces)
    {
        _numberOfDecimalPlaces = numberOfDecimalPlaces;
    }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        return (((string)value).AsDecimal(_numberOfDecimalPlaces) != null);
    }
}