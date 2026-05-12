using MilkTea.Data;
using MilkTea.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Dòng này phải nằm ở đây mới đúng
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MilkTea.Controllers
{

    public class ProductsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Trang danh sách sản phẩm


        // 2. Mở Form Thêm Mới (GET)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // 3. Lưu sản phẩm mới (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                // 1. Kiểm tra nếu anh có chọn file ảnh
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Tạo tên file duy nhất (để không bị trùng)
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);

                    // Đường dẫn vật lý để lưu file vào thư mục wwwroot/images
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    // Lưu file vào thư mục
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    // Lưu đường dẫn ảnh vào Database (ví dụ: /images/abc.jpg)
                    product.ImageUrl = "/images/" + fileName;
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // 5. Lưu dữ liệu sau khi sửa (POST)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return NotFound();

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
                return View(product);
            }
        }

        // 6. Xóa món
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        // Thay thế hàm Index cũ bằng hàm này ở ngay đầu class ProductsController
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            // 1. Lấy danh sách sản phẩm và nạp luôn bảng Loại hàng (Category) vào
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            // 2. Lọc theo tên nếu anh gõ vào ô tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }

            // 3. Lọc theo loại đồ uống
            if (categoryId.HasValue)
            {
                products = products.Where(x => x.CategoryId == categoryId);
            }

            // 4. Chuẩn bị danh sách loại hàng để hiện lên ô chọn (Dropdown)
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            ViewData["CurrentFilter"] = searchString;
            // Đếm tổng số món dựa trên danh sách tạm thời trong CartController
            // Lưu ý: CartController._cart phải là static thì mới gọi trực tiếp được như thế này
            ViewBag.CartCount = CartController._cart.Sum(x => x.Quantity);
            // Anh cần khai báo lại hoặc dùng Session để chuẩn nhất, nhưng hiện tại cứ để hiện icon đã!
            return View(await products.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

    }
}