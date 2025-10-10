using System;
using System.Globalization;

namespace Recruit.Shared.Web.Extensions;

public static class StringExtensions
{
    private static readonly IFormatProvider _ukCulture = new CultureInfo("en-GB");
        
    public static DateTime? AsDateTimeUk(this string date)
    {
        if(DateTime.TryParseExact(date, "d/M/yyyy", _ukCulture, DateTimeStyles.AssumeUniversal, out var d))
        {
            return d;
        }

        return null;
    }

    public static string RemoveOxfordComma(this string text)
    {
        return text.Replace(", and ", " and ");
    }
		
    public static bool IsEqualWithoutSymbols(this string source, string target)
    {
        if (target == null) return false;
        return String.Compare(source, target, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols) == 0;
    }
}