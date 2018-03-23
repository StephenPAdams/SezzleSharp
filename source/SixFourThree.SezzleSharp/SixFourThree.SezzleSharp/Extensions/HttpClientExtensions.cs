using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SixFourThree.SezzleSharp.Models.Responses;

namespace SixFourThree.SezzleSharp.Extensions
{
    internal static class HttpClientExtensions
    {
        public static async Task<T> ExecuteAsync<T>(this HttpClient client, HttpRequestMessage request) where T: Response, new()
        {
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode == false && response.Content.Headers.ContentType.MediaType != "application/json")
            {
                response.EnsureSuccessStatusCode();
            }

            string resultData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(resultData, SezzleConfig.DefaultSerializerSettings);

            var endpointResponse = result as Response;

            if (endpointResponse == null)
            {
                // Some calls from Sezzle return the HTTP status code in the body...some opt for an empty body but instead return it in the header.
                result = new T { HttpStatusCode = response.StatusCode, Status = (int) response.StatusCode, Message = response.ReasonPhrase };
            }

            return result;
        }
    }
}
