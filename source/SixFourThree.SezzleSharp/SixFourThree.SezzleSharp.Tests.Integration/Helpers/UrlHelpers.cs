using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SixFourThree.SezzleSharp.Tests.Integration.Helpers
{
    public static class UrlHelpers
    {
        public static string GetQueryStringValue(this string url, string queryIndex)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            
            return query.Get(queryIndex);
        }
    }
}
