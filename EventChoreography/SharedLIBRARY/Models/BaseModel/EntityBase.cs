using SharedLIBRARY.Enums;

namespace SharedLIBRARY.Models.BaseModel
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string? ErrorMessage { get; set; } = string.Empty;
    }
}
