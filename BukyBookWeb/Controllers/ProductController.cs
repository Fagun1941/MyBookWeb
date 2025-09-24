using BukyBookWeb.Helpers;
using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog.Context;
using System.Net;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            try
            {
                int pageSize = 3;
                var products = await _productService.GetAllProductAsync(search, page, pageSize);
                int totalProducts = await _productService.GetTotalCountProductAsync(search);

                ViewBag.PageNumber = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
                ViewBag.Search = search;

                return View(products);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                    _logger.LogError(ex, "Error loading product list | Search: {Search} | Page: {Page} | CorrelationId={LogGuid}", search, page, logGuid);

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Loading Product. Tracking ID: {logGuid}");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.Categories = new SelectList(await _productService.GetCategoriesAsync(), "Id", "Name");
                _logger.LogInformation("Opened Create Product page");
                Response.StatusCode = (int)HttpStatusCode.OK;
                return View();
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                    _logger.LogError(ex, "Error loading Create page | CorrelationId={LogGuid}", logGuid);

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Loading Create. Tracking ID: {logGuid}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productService.AddProductAsync(product, file);
                    _logger.LogInformation("Created Product: {ProductName}", product.Title);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning("Create Product failed due to invalid model state");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Categories = new SelectList(await _productService.GetCategoriesAsync(), "Id", "Name", product.CategoryId);
                return View(product);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                    _logger.LogError(ex, "Error creating product {ProductName} | CorrelationId={LogGuid}", product.Title, logGuid);

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Create Product. Tracking ID: {logGuid}");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditProduct(int id)
        {
            try
            {
                var product = await _productService.GetByIdProductAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product not found for edit | Id: {Id}", id);
                    return this.HandleError(HttpStatusCode.NotFound, "Product not found");
                }

                ViewBag.Categories = new SelectList(await _productService.GetCategoriesAsync(), "Id", "Name", product.CategoryId);
                _logger.LogInformation("Opened Edit page for Product Id: {Id}", id);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return View(product);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                    _logger.LogError(ex, "Error loading Edit page for Product Id: {Id} | CorrelationId={LogGuid}", id, logGuid);

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error loading product for edit. Tracking ID: {logGuid}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productService.UpdateProductAsync(product, file);
                    _logger.LogInformation("Updated Product: {ProductName} (Id: {Id})", product.Title, product.Id);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning("Update Product failed due to invalid model state");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Categories = new SelectList(await _productService.GetCategoriesAsync(), "Id", "Name", product.CategoryId);
                return View(product);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                    _logger.LogError(ex, "Error updating product {ProductName} (Id: {Id}) | CorrelationId={LogGuid}", product.Title, product.Id, logGuid);

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Updating Product. Tracking ID: {logGuid}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productService.GetByIdProductAsync(id);
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
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                    _logger.LogError(ex, "Error loading product details | Id: {Id} | CorrelationId={LogGuid}", id, logGuid);

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Loading Details Product. Tracking ID: {logGuid}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                _logger.LogInformation("Deleted Product | Id: {Id}", id);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                    _logger.LogError(ex, "Error deleting product | Id: {Id} | CorrelationId={LogGuid}", id, logGuid);

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Deleting Product. Tracking ID: {logGuid}");
            }
        }
    }
}
