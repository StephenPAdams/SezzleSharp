using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Exceptions;
using SixFourThree.SezzleSharp.Interfaces;

namespace SixFourThree.SezzleSharp.Models.Configuration
{
    public class ConfigurationUpdateResponse : ITransparentEndpointResponse
    {
        public RequestAndResponseInfo RequestAndResponseInfo { get; set; }
    }
}
