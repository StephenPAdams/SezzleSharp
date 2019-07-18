using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Exceptions;
using SixFourThree.SezzleSharp.Exceptions.Specific;
using SixFourThree.SezzleSharp.Interfaces;

namespace SixFourThree.SezzleSharp.Extensions
{
    public class SezzleHttpClient : ISezzleHttpClient
    {
        private SezzleHttpClientConfiguration _sezzleHttpClientConfiguration = null;

        public SezzleHttpClient(SezzleHttpClientConfiguration configuration)
        {
            _sezzleHttpClientConfiguration = configuration;
        }

        private async Task<TK> SezzleRestCall<T, TK>(string authenticationToken, string requestUrl, T requestObject, RestSharp.Method httpMethod, List<KeyValuePair<string, string>> parameters)
            where T : class
            where TK : class, ITransparentEndpointResponse, new()
        {
            //setup this object so we have a history of what has been done
            var rarInfo = new RequestAndResponseInfo()
            {
                RequestUrl = requestUrl
            };

            var request = CreateRestRequest<T, TK>(authenticationToken, requestObject, httpMethod, parameters, rarInfo);

            var restClient = new RestClient(requestUrl);
            restClient.Timeout = (int)_sezzleHttpClientConfiguration.Timeout.TotalMilliseconds;

            IRestResponse response = null;

            //execute request
            try
            {
                //not sure what was going on there, but it was not coming back with auth tokens sometimes when async.
                //  response = await restClient.ExecuteTaskAsync(request);
                response = restClient.Execute(request);
                rarInfo.ResponseBody = response.Content;
                rarInfo.ResponseHttpStatusCode = response.StatusCode;
            }
            catch (Exception ex)
            {
                throw new SezzleCommunicationException("Error communicating with Sezzle endpoint.", ex)
                {
                    RequestAndResponseInfo = rarInfo
                };
            }

            //if successful, deserialize as expected object type and return.
            if (response.IsSuccessful)
            {
                try
                {
                    //attempt to deserialize the response as expected return object type
                    var respObject = JsonConvert.DeserializeObject<TK>(response.Content, SezzleConfig.DefaultSerializerSettings);
                    //some responses just return a 200 and no data (body length of 0).  return a new instance of the response object.
                    if (respObject == null)
                    {
                        respObject = new TK();
                    }

                    respObject.RequestAndResponseInfo = rarInfo;

                    return respObject;
                }
                catch (Exception ex)
                {
                    throw new SezzleUnexpectedResponseException("Sezzle response type did not match expected return type.", ex)
                    {
                        RequestAndResponseInfo = rarInfo
                    };
                }
            }

            //if we got here, the request was not successful.
            throw new SezzleErrorResponseException("Sezzle responded unfavorably to the request.")
            {
                RequestAndResponseInfo = rarInfo
            };

        }

        private static RestRequest CreateRestRequest<T, TK>(string authenticationToken, T requestObject, Method httpMethod,
            List<KeyValuePair<string, string>> parameters, RequestAndResponseInfo rarInfo)
            where T : class where TK : class, ITransparentEndpointResponse, new()
        {
            var request = new RestRequest()
            {
                RequestFormat = DataFormat.Json,
                Method = httpMethod
            };

            //add parameters.  currently only needed for ONE of the many calls.
            foreach (var p in parameters ?? new List<KeyValuePair<string, string>>())
            {
                request.AddParameter(p.Key, p.Value);
            }

            //we are using RestSharp to do the calls but note that we are serializing our own requests and responses using the Newtonsoft.JSON package so that we can use Newtonsoft's SnakeCaseSerializer.
            if (requestObject != null)
            {
                var body = rarInfo.RequestBody =
                    JsonConvert.SerializeObject(requestObject, SezzleConfig.DefaultSerializerSettings);
                request.AddJsonBody(body);
            }

            //add authorization
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                request.AddParameter("Authorization", $"Bearer {authenticationToken}", ParameterType.HttpHeader);
            }

            return request;
        }

        public async Task<TK> PostAsync<T, TK>(string authenticationToken, string requestUrl, T requestObject, List<KeyValuePair<string, string>> parameters = null) where T : class where TK : class, ITransparentEndpointResponse, new()
        {
            return await SezzleRestCall<T, TK>(authenticationToken, requestUrl, requestObject, Method.POST, parameters);
        }

        public async Task<TK> GetAsync<T, TK>(string authenticationToken, string requestUrl, T requestObject, List<KeyValuePair<string, string>> parameters = null) where T : class where TK : class, ITransparentEndpointResponse, new()
        {
            return await SezzleRestCall<T, TK>(authenticationToken, requestUrl, requestObject, Method.GET, parameters);
        }
    }
}
