using Newtonsoft.Json;
using SixFourThree.SezzleSharp.Models;

namespace SixFourThree.SezzleSharp.Extensions
{
    public static class SezzleErrorResponseExtensions
    {
        /// <summary>
        /// Attempts to deserialize a string into an instance of SezzleGenericErrorResponse.  If deserialization fails, returns null.
        /// </summary>
        /// <param name="httpResponseBody"></param>
        /// <returns></returns>
        public static SezzleGenericErrorResponse AsSezzleGenericErrorResponse(this string httpResponseBody)
        {
            SezzleGenericErrorResponse resp = null;

            try
            {
                resp = JsonConvert.DeserializeObject<SezzleGenericErrorResponse>(httpResponseBody, SezzleConfig.DefaultSerializerSettings);
            }
            catch
            {
                //do nothing; deserialization failed so return null.
            }

            return resp;
        }
    }
}
