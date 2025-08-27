using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Recruit.Shared.Web.ViewModels;

public class ValidationSummaryViewModel
{
    public IList<string> OrderedFieldNames { get; set; } = new List<string>();
    public ModelStateDictionary ModelState { get; set; }
}