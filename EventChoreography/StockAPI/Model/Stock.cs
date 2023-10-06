using SharedLIBRARY.Models.BaseModel;

namespace StockAPI.Model
{
    public class Stock : EntityBase
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}
