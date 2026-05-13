using System.ComponentModel.DataAnnotations;

namespace MilkTea.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại không được để trống")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}