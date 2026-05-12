using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTea.Data;
using MilkTea.Models;
using System.Diagnostics;

namespace MilkTea.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Bước quan trọng: Phải có chữ async và Task<IActionResult>
        public IActionResult Index()
        {
            return View();
        }

        // Trang Thực đơn: Lấy danh sách món từ SQL và gửi sang View Menu
        public async Task<IActionResult> Menu(int? categoryId, string searchTerm)
        {
            // 1. Lấy danh sách Categories để hiện lên các nút bấm (Tab)
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;

            // 2. Bắt đầu truy vấn sản phẩm
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            // 3. Lọc theo danh mục nếu có chọn
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            // 4. Lọc theo từ khóa tìm kiếm nếu có nhập
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm));
            }

            var resultList = await query.ToListAsync();
            return View(resultList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}