﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SixFourThree.SezzleSharp.Models.Responses
{
    public class CheckoutResponse : Response
    {
        public string CheckoutUrl { get; set; }
    }
}
