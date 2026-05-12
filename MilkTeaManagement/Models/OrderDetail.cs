// Thay chữ "MilkTeaManagement" bằng tên Project thật của anh (xem ở Bước 1)
using MilkTea.Models;
namespace MilkTea.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Kết nối với bảng khác
        public Order? Order { get; set; }

        // Nếu dòng này vẫn đỏ, anh nhấn Ctrl + . (dấu chấm) 
        // rồi chọn "using [Tên Project của anh].Models"
        public Product? Product { get; set; }
    }
}