using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Discount")]
    public class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Percent { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public DateTime DeletetedAt { get; set; } = DateTime.Now;
    }
}
