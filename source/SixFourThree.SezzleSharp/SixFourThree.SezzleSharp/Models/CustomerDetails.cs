using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SixFourThree.SezzleSharp.Models
{
    public class CustomerDetails
    {
        /// <summary>
        /// The user's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's phone number
        /// </summary>
        public string Phone { get; set; }
    }
}
