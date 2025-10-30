using System;

namespace Recruit.Shared.Web.Helpers;

public static class PagingHelper
{
    public static int GetTotalNoOfPages(int pageSize, int noOfTotalResults)
    {
        return noOfTotalResults == 0
            ? 0
            : (int)Math.Ceiling(noOfTotalResults / (double)pageSize);
    }
}