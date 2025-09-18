using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using BukyBookWeb.Helpers;

namespace BukyBookWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger; 

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        public IActionResult Index(string? search, int page = 1)
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

                _logger.LogInformation("Loaded product list | Search: {Search} | Page: {Page}", search, page);

                Response.StatusCode = (int)HttpStatusCode.OK;
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product list | Search: {Search} | Page: {Page}", search, page);
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Loading Product: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            try
            {
                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name");

                _logger.LogInformation("Opened Create Product page");

                Response.StatusCode = (int)HttpStatusCode.OK;
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Create page");
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Loading Create: {ex.Message}");
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
                    _logger.LogInformation("Created Product: {ProductName}", product.Title);

                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning("Create Product failed due to invalid model state");

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product {ProductName}", product.Title);
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Create Product: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditProduct(int id)
        {
            try
            {
                var product = _productService.GetByIdProduct(id);
                if (product == null)
                {
                    _logger.LogWarning("Product not found for edit | Id: {Id}", id);
                    return this.HandleError(HttpStatusCode.NotFound, "Product not found");
                }

                _logger.LogInformation("Opened Edit page for Product Id: {Id}", id);

                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Edit page for Product Id: {Id}", id);
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error loading product for edit: {ex.Message}");
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
                    _logger.LogInformation("Updated Product: {ProductName} (Id: {Id})", product.Title, product.Id);

                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning("Update Product failed due to invalid model state");

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductName} (Id: {Id})", product.Title, product.Id);
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Updating Product: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var product = _productService.GetByIdProduct(id);
                if (product == null)
                {
                    _logger.LogWarning("Product not found for details | Id: {Id}", id);
                    return this.HandleError(HttpStatusCode.NotFound, "Product not found");
                }

                _logger.LogInformation("Viewed Product Details | Id: {Id}", id);

                Response.StatusCode = (int)HttpStatusCode.OK;
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product details | Id: {Id}", id);
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Loading Details Product: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                _logger.LogInformation("Deleted Product | Id: {Id}", id);

                Response.StatusCode = (int)HttpStatusCode.OK;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product | Id: {Id}", id);
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Deleting Product: {ex.Message}");
            }
        }
    }
}
