using System.ComponentModel.DataAnnotations;

namespace MilkTea.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại không được để trống")]
        [MaxLength(100)]
        // THÊM = string.Empty; Ở ĐÂY 👇
        public string Name { get; set; } = string.Empty;

        // THÊM = new List<Product>(); Ở ĐÂY 👇
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}