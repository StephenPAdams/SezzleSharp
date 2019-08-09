using System;
using SixFourThree.SezzleSharp.Endpoints;
using SixFourThree.SezzleSharp.Exceptions;
using SixFourThree.SezzleSharp.Interfaces;

namespace SixFourThree.SezzleSharp.Models.Authentication
{
    public class AuthenticationResponse: ITransparentEndpointResponse
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public RequestAndResponseInfo RequestAndResponseInfo { get; set; }
    }
}
