using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Recruit.Shared.Web.ModelBinders;

namespace Recruit.Shared.Web.Extensions;

public static class MvcOptionsExtensions
{
    public static void AddTrimModelBinderProvider(this MvcOptions option, ILoggerFactory loggerFactory)
    {
        var binderToFind = option.ModelBinderProviders
            .FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));
        if (binderToFind == null)
        {
            return;
        }
        var index = option.ModelBinderProviders.IndexOf(binderToFind);
        option.ModelBinderProviders.Insert(index, new TrimModelBinderProvider(loggerFactory));
    }
}