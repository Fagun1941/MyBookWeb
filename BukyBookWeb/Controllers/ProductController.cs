using BukyBookWeb.Data;
using BukyBookWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;

    public ProductController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var products = _db.Products.Include(p => p.Category).ToList();
        return View(products);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");

        return View();
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            if (file != null && file.Length > 0)
            {
                // Generate unique filename
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // Path to wwwroot/images/products
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);

                // Save file to server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Save relative path to DB
                product.ImageUrl = "/images/products/" + fileName;
            }

            _db.Products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name", product.CategoryId);
        return View(product);
    }

    [HttpGet]

    public IActionResult Details(int Id)
    {
        var product = _db.Products.Include(p => p.Category).FirstOrDefault(i => i.Id == Id);

        if (product == null)
        {
            return NotFound(); 
        }

        return View(product);
    }

}
