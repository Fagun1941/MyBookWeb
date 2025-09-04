using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BukyBookWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index(string search)
        {
            var products = _productService.GetAllProduct(search);
            return View(products);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProduct(product, file);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditProduct(int id)
        {
            var product = _productService.GetByIdProduct(id);
            if (product == null) return NotFound();

            ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(product, file);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _productService.GetByIdProduct(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
