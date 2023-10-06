using Microsoft.EntityFrameworkCore;

namespace OrderAPI.Models
{
    [Owned]
    public class Address
    {
        public Address()
        {
        }

        public Address(string country, string city, string neighbourhood,string fullAddress)
        {
            Country = country;
            City = city;
            FullAddress = fullAddress;
            Neighbourhood = neighbourhood;
        }

        public string Country { get; set; }
        public string City { get; set; }
        public string Neighbourhood { get; set; }
        public string FullAddress { get; set; }


        public static Address AddAddress(string country, string city, string neighbourhood, string fullAddress)
        {
            return new Address(country,city,neighbourhood, fullAddress);
        }
    }
}
