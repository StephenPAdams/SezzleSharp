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
        /// <remarks>Required</remarks>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        /// <remarks>Required</remarks>
        public string LastName { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        /// <remarks>Required</remarks>
        public string Email { get; set; }

        /// <summary>
        /// The user's phone number
        /// </summary>
        public string Phone { get; set; }

        public CustomerDetails() { }

        public CustomerDetails(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
