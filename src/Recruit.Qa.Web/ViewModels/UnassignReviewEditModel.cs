using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Recruit.Qa.Web.ViewModels.Validations;

namespace Recruit.Qa.Web.ViewModels;

public class UnassignReviewEditModel
{
    [FromRoute]
    public Guid ReviewId { get; set; }

    [Required(ErrorMessage = ValidationMessages.UnassignReviewConfirmationMessages.SelectionRequired)]
    public bool? ConfirmUnassign { get; set; }
}