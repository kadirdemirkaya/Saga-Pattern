using SharedLIBRARY.Enums;
using SharedLIBRARY.Models.BaseModel;

namespace OrderAPI.Models
{
    public class Order : EntityBase
    {
        public int BasketId { get; set; }
        public OrderStatus Status { get; set; }
        public Address Address { get; set; }


        public static Order Create(int basketId,OrderStatus status)
        {
            return new Order { BasketId = basketId, Status = status };
        }

        public Order AddAddress(string country,string city,string neighbourhood,string fullAddress)
        {
            Address = new Address(country, city, neighbourhood, fullAddress);
            return this;
        }
    }
}
