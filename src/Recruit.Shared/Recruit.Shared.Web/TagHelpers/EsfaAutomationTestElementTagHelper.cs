﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Recruit.Shared.Web.Configuration;

namespace Recruit.Shared.Web.TagHelpers;

[HtmlTargetElement(Attributes = TagAttributeName)]
public class EsfaAutomationTestElementTagHelper(IWebHostEnvironment env) : TagHelper
{
    private const string TagAttributeName = "esfa-automation";
    private const string DataAutomationAttributeName = "data-automation";

    [HtmlAttributeName(TagAttributeName)]
    public string TargetName { get; set; }
        
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!env.IsEnvironment(EnvironmentNames.PROD))
        {
            output.Attributes.SetAttribute(DataAutomationAttributeName, TargetName);
        }

        return Task.CompletedTask;
    }
}