using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Product_Inventory")]
    public class ProductInventory
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public DateTime DeletetedAt { get; set; } = DateTime.Now;
    }
}
