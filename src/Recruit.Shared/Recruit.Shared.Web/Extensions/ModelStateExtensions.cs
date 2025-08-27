using Microsoft.AspNetCore.Mvc.ModelBinding;
using Recruit.Shared.Web.Exceptions;

namespace Recruit.Shared.Web.Extensions;

public static class ModelStateExtensions
{
    public static void ThrowIfBindingErrors(this ModelStateDictionary modelState)
    {
        if (modelState == null || modelState.IsValid)
        {
            return;
        }

        throw new ModelBindingException(modelState);
    }
}