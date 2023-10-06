namespace SharedLIBRARY.Events
{
    public class StockNotEnoughtEvent
    {
        public int OrderId { get; set; }
        public int BasketId { get; set; }
    }
}
