using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        public string ProductSymbol { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public DateTime DeletetedAt { get; set; } = DateTime.Now;
        public string Category_Id { get; set; }
        public ProductCategory Category { get; set; }
        public string Inventory_Id { get; set; }
        public ProductInventory Inventory { get; set; }
        public string Discount_Id { get; set; }
        public Discount discount { get; set; }
    }
}
