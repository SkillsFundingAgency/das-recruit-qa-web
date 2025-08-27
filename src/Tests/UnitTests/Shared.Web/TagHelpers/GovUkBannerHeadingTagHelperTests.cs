using NUnit.Framework;
using Recruit.Shared.Web.TagHelpers;

namespace Recruit.Qa.Vacancies.Client.UnitTests.Shared.Web.TagHelpers;

public class GovUkBannerHeadingTagHelperTests: TagHelperTestsBase
{
    [Test]
    public async Task Output_Renders_Correctly()
    {
        // arrange
        var sut = new GovUkBannerHeadingTagHelper();

        // act
        await sut.ProcessAsync(TagHelperContext, TagHelperOutput);

        // assert
        TagHelperOutput.AsString().Should().Be("<h3 class=\"HtmlEncode[[govuk-notification-banner__heading]]\">default content</h3>");
    }
}