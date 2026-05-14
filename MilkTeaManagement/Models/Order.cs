using System;
using System.ComponentModel.DataAnnotations;

namespace MilkTea.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string? UserId { get; set; } 
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên khách hàng")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        public string Status { get; set; } = "Chờ xác nhận";
        [Required]
        public string PaymentMethod { get; set; } = "Tien mat";
    }
}