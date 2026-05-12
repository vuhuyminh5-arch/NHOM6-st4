using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        // THÊM = string.Empty; Ở ĐÂY 👇
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        // THÊM = string.Empty; Ở ĐÂY 👇
        public string? ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = null!; // Thêm = null! để báo hệ thống bỏ qua cảnh báo này
    }
}