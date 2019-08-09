using System.Collections.Generic;
using System.Threading.Tasks;

namespace SixFourThree.SezzleSharp.Interfaces
{
    public interface ISezzleHttpClient
    {
        Task<TK> PostAsync<T, TK>(string authenticationToken, string requestUrl, T requestObject,
            List<KeyValuePair<string, string>> parameters = null)
            where T : class
            where TK : class, ITransparentEndpointResponse, new();

        Task<TK> GetAsync<T, TK>(string authenticationToken, string requestUrl, T requestObject,
            List<KeyValuePair<string, string>> parameters = null)
            where T : class
            where TK : class, ITransparentEndpointResponse, new();
    }
}
