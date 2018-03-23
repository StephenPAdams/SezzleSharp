using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SixFourThree.SezzleSharp.Models.Responses
{
    public class AuthResponse : Response
    {
        public string Token { get; set; }
        
        public DateTime ExpirationDate { get; set; }
    }
}
