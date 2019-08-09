namespace SixFourThree.SezzleSharp.Models.Checkouts.Supporting
{
    public class Address
    {
        /// <summary>
        /// The name on the address
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The street and number of the address
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// The apt or unit
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// The city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The 2 character state code
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The postal delivery code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// The 2 character country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// The 2 character country code
        /// </summary>
        public string PhoneNumber { get; set; }

        public Address() { }

        public Address(string name, string street, string city, string state, string postalCode, string countryCode)
        {
            Name = name;
            Street = street;
            City = city;
            State = state;
            PostalCode = postalCode;
            CountryCode = countryCode;
        }
    }
}
