using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Exceptions;
using SixFourThree.SezzleSharp.Interfaces;

namespace SixFourThree.SezzleSharp.Models.Health
{
    public class HealthCheckResponse:ITransparentEndpointResponse
    {
        public RequestAndResponseInfo RequestAndResponseInfo { get; set; }
    }
}
