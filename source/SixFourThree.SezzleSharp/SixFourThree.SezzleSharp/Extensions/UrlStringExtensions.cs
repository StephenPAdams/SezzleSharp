using System;
using System.Collections.Generic;
using System.Text;
using Flurl;

namespace SixFourThree.SezzleSharp.Extensions
{
    public class UrlStringExtensions
    {
        public static string FormatRequestUrl(string baseUrl, string pathAndQuery)
        {
            char[] charsToTrim = { '/' };

            baseUrl = baseUrl.Replace('\\', '/');
            pathAndQuery = pathAndQuery.Replace('\\', '/');

            var url = Url
                .Combine(
                    baseUrl.Trim().TrimStart(charsToTrim).TrimEnd(charsToTrim).Trim(),
                    pathAndQuery.Trim().TrimStart(charsToTrim).TrimEnd(charsToTrim).Trim()
                );

            url = url.TrimEnd('/');

            return url;
        }
    }
}
