using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace BukyBookWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index(string search, int page = 1)
        {
            try
            {
                int pageSize = 3;
                var products = _productService.GetAllProduct(search, page, pageSize);

                int totalProducts = _productService.GetTotalCountProduct(search);
                ViewBag.PageNumber = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
                ViewBag.Search = search;

                return View(products);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Loading Product: {ex.Message}"
                };

                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            try
            {
                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name");
                return View();
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Loading Create: {ex.Message}"
                };

                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _productService.AddProduct(product, file);
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
                return View(product);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Create Product: {ex.Message}"
                };

                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditProduct(int id)
        {
            try
            {
                var product = _productService.GetByIdProduct(id);
                if (product == null) return NotFound();

                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
                return View(product);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error loading product for edit: {ex.Message}"
                };

                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _productService.UpdateProduct(product, file);
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
                return View(product);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Updating Product: {ex.Message}"
                };

                return View("Error", errorModel);
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _productService.GetByIdProduct(id);
                if (product == null) return NotFound();
                return View(product);
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Loading Details Product: {ex.Message}"
                };

                return View("Error", errorModel);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Loading Product: {ex.Message}"
                };

                return View("Error", errorModel);
            }
        }
    }
}
