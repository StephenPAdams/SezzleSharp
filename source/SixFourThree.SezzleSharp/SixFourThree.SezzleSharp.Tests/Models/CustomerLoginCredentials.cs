using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixFourThree.SezzleSharp.Tests.Models
{
 public   class CustomerLoginCredentials
    {
        public string PhoneNumber { get; set; }
        public string Pin { get; set; }
        public string OtpCode { get; set; }
    }
}
