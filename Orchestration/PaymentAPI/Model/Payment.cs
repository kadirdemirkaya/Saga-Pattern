using SharedLIBRARY.Enums;
using SharedLIBRARY.Models.BaseModel;

namespace PaymentAPI.Model
{
    public class Payment : EntityBase
    {
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CardholderName { get; set; }
        public string CardholderLastname { get; set; }
        public string CVV { get; set; }
        public double Balance { get; set; }
        public PaymentStatus Status { get; set; }


        public int CustomerId { get; set; }
    }
}
