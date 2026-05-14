using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTea.Data;
using MilkTea.Models;
using System.Text.Json;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private const string CartSessionKey = "Cart";

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ── Helper: lấy giỏ hàng từ Session ──────────────────────────
    private List<CartItem> GetCart()
    {
        var json = HttpContext.Session.GetString(CartSessionKey);
        return json == null ? new List<CartItem>()
                            : JsonSerializer.Deserialize<List<CartItem>>(json)!;
    }

    // ── Helper: lưu giỏ hàng vào Session ─────────────────────────
    private void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetString(CartSessionKey, JsonSerializer.Serialize(cart));
    }

    // ── Xem giỏ hàng ─────────────────────────────────────────────
    public IActionResult Index()
    {
        return View(GetCart());
    }

    // ── Thêm vào giỏ ─────────────────────────────────────────────
    public IActionResult AddToCart(int id, int quantity = 1)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();

        // Đảm bảo quantity hợp lệ
        if (quantity < 1) quantity = 1;
        if (quantity > 99) quantity = 99;

        var cart = GetCart();
        var item = cart.FirstOrDefault(c => c.ProductId == id);

        if (item == null)
        {
            cart.Add(new CartItem
            {
                ProductId = id,
                ProductName = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = quantity
            });
        }
        else
        {
            item.Quantity += quantity;
            if (item.Quantity > 99) item.Quantity = 99;
        }

        SaveCart(cart);
        TempData["SuccessMsg"] = $"Đã thêm {quantity} x {product.Name} vào giỏ!";
        return RedirectToAction("Menu", "Home");
    }

    // ── Xóa khỏi giỏ ─────────────────────────────────────────────
    public IActionResult Remove(int id)
    {
        var cart = GetCart();
        cart.RemoveAll(c => c.ProductId == id);
        SaveCart(cart);
        return RedirectToAction("Index");
    }

    // ── Trang Checkout (GET) ──────────────────────────────────────
    public IActionResult Checkout()
    {
        var cart = GetCart();
        if (!cart.Any()) return RedirectToAction("Index");

        ViewBag.CartItems = cart;
        ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);
        return View();
    }

    // ── Xử lý đặt hàng (POST) ────────────────────────────────────
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Checkout(Order order)
    {
        var cart = GetCart();

        if (!ModelState.IsValid)
        {
            ViewBag.CartItems = cart;
            ViewBag.Total = cart.Sum(x => x.Price * x.Quantity);
            return View(order);
        }

        // Gán UserId của người đang đăng nhập
        order.UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        order.TotalAmount = cart.Sum(x => x.Total);
        order.OrderDate = DateTime.Now;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        foreach (var item in cart)
        {
            _context.OrderDetails.Add(new OrderDetail
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            });
        }
        await _context.SaveChangesAsync();

        // Xóa giỏ hàng của đúng user này thôi
        SaveCart(new List<CartItem>());

        return RedirectToAction("OrderSuccess");
    }

    // ── Danh sách đơn hàng (Admin) ────────────────────────────────
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> OrderList()
    {
        var orders = await _context.Orders
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
        return View(orders);
    }

    // ── Chi tiết đơn hàng ─────────────────────────────────────────
    public async Task<IActionResult> OrderDetails(int id)
    {
        var details = await _context.OrderDetails
            .Include(d => d.Product)
            .Where(d => d.OrderId == id)
            .ToListAsync();
        return View(details);
    }

    // ── Cập nhật trạng thái (POST, Admin only) ────────────────────
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            order.Status = status;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("OrderList");
    }

    public IActionResult OrderSuccess() => View();
}