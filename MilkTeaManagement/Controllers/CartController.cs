using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTea.Data; // Nhớ đổi tên theo project của anh
using MilkTea.Models;
using Microsoft.AspNetCore.Authorization;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    // Em dùng một danh sách tạm thời (Static) để anh Minh dễ test nhanh không cần cài Session phức tạp
    public static List<CartItem> _cart = new List<CartItem>();

    public CartController(ApplicationDbContext context) { _context = context; }
    // Thêm hàm này vào để mở được trang Giỏ hàng
    public IActionResult Index()
    {
        return View(_cart);
    }
    public IActionResult AddToCart(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound(); // Thêm dòng này để bảo vệ hệ thống

        var item = _cart.FirstOrDefault(c => c.ProductId == id);
        if (item == null)
        {
            _cart.Add(new CartItem
            {
                ProductId = id,
                ProductName = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = 1
            });
        }
        else
        {
            item.Quantity++;
        }
        TempData["SuccessMsg"] = $"Đã thêm {product.Name} vào giỏ!";
        // Quay lại trang Thực đơn sau khi thêm món thành công
        return RedirectToAction("Menu", "Home");
    }

    public IActionResult Remove(int id)
    {
        _cart.RemoveAll(c => c.ProductId == id);
        // Sửa dòng cuối cùng trong hàm AddToCart TempDat   a


        // Quay lại trang Thực đơn sau khi thêm món thành công
        return RedirectToAction("Menu", "Home");
    }
    [HttpGet]
    public IActionResult Checkout()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Checkout(Order order)
    {
        if (string.IsNullOrEmpty(order.Address))
        {
            order.Address = "Địa chỉ chưa xác định";
        }
        // 1. Tính tổng tiền
        order.TotalAmount = _cart.Sum(x => x.Total);
        order.OrderDate = DateTime.Now;

        // 2. Lưu thông tin đơn hàng chính
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // 3. Lưu chi tiết từng món vào bảng OrderDetail
        foreach (var item in _cart)
        {
            var detail = new OrderDetail
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            };
            _context.OrderDetails.Add(detail);
        }
        await _context.SaveChangesAsync();

        // 4. Xóa giỏ hàng sau khi mua xong
        _cart.Clear();

        return Content("Cảm ơn anh Minh! Đơn hàng đã được ghi nhận thành công.");
    }
    // Hàm hiển thị danh sách đơn hàng cho anh Minh quản lý*
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> OrderList()
    {
        // Lấy toàn bộ đơn hàng từ bảng Orders, sắp xếp theo ngày mới nhất
        var orders = await _context.Orders
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return View(orders);
    }
    public async Task<IActionResult> OrderDetails(int id)
    {
        // Lấy chi tiết đơn hàng kèm theo thông tin Sản phẩm
        var details = await _context.OrderDetails
            .Include(d => d.Product)
            .Where(d => d.OrderId == id)
            .ToListAsync();

        return View(details);
    }
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            order.Status = status; // Cập nhật trạng thái mới
            await _context.SaveChangesAsync();
        }
        // Quay lại trang danh sách đơn hàng
        return RedirectToAction("OrderList");
    }
}
