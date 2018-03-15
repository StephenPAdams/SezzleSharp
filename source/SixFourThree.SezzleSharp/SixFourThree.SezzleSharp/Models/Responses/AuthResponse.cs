using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SixFourThree.SezzleSharp.Models.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; }

        [JsonProperty("expiration_date")]
        public DateTime ExpirationDate { get; set; }
    }
}
