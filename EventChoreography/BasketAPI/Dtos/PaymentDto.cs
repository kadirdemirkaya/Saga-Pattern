namespace BasketAPI.Dtos
{
    public class PaymentDto
    {
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CardholderName { get; set; }
        public string CardholderLastname { get; set; }
        public string CVV { get; set; }
    }
}
