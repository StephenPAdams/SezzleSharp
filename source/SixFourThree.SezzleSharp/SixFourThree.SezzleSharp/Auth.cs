using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp
{
    /// <summary>
    /// Sezzle Pay uses scoped API keys to allow access to the API. You can find/generate these keys on your merchant dashboard once you have been approved by Sezzle.
    /// Once you have a valid token, it must be used as a Header for subsequent requests to their API, using the format below.
    /// Authorization: Bearer authToken
    /// </summary>
    public class Auth
    {
        private readonly SezzleConfig config;
    }
}
