using System;
using System.Collections.Generic;
using System.Text;

namespace SixFourThree.SezzleSharp.Extensions
{
   public class SezzleHttpClientConfiguration
    {
        public TimeSpan Timeout { get; set; }

        public SezzleHttpClientConfiguration()
        {
            //default to 30 seconds.
            Timeout = TimeSpan.FromSeconds(30);
        }
    }
}
