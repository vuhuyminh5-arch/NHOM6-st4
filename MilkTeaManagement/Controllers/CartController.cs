using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTea.Data; 
using MilkTea.Models;
using System.Linq;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    public static List<CartItem> _cart = new List<CartItem>();

    public CartController(ApplicationDbContext context) { _context = context; }
    public IActionResult Index()
    {
        return View(_cart);
    }
    public IActionResult AddToCart(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound(); 

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
        return RedirectToAction("Menu", "Home");
    }

    public IActionResult Remove(int id)
    {
        _cart.RemoveAll(c => c.ProductId == id);
      


        return RedirectToAction("Menu", "Home");
    }
    public IActionResult Checkout()
    {
        var cartItems = GetCartItems();

        if (cartItems == null || !cartItems.Any())
        {
            return RedirectToAction("Index");
        }

        ViewBag.CartItems = cartItems;
        ViewBag.Total = cartItems.Sum(x => x.Price * x.Quantity);

        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Checkout(Order order)
    {
        if (!ModelState.IsValid)
        {
            return View(order);
        }

        if (string.IsNullOrEmpty(order.Address))
        {
            order.Address = "Địa chỉ chưa xác định";
        }
        order.TotalAmount = _cart.Sum(x => x.Total);
        order.OrderDate = DateTime.Now;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

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

        _cart.Clear();

        return RedirectToAction("OrderSuccess");
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> OrderList()
    {
        var orders = await _context.Orders
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return View(orders);
    }
    public async Task<IActionResult> OrderDetails(int id)
    {
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
            order.Status = status; 
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("OrderList");
    }
    public IActionResult OrderSuccess()
    {
        return View();
    }


    private List<CartItem> GetCartItems()
    {
        return _cart;
    }
}
