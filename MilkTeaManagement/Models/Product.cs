using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTea.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Anh Minh ơi, đừng quên đặt tên cho món nhé!")]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Giá không thể âm được đâu anh!")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; } 

        public int CategoryId { get; set; } 

        public virtual Category? Category { get; set; }
    }
}

