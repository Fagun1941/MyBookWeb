using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Net;
using BukyBookWeb.Helpers;


namespace BukyBookWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
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

                Response.StatusCode = (int)HttpStatusCode.OK;
                return View(products);
            }
            catch (Exception ex)
            {
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Loading Product: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            try
            {
                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name");
                Response.StatusCode = (int)HttpStatusCode.OK;
                return View();
            }
            catch (Exception ex)
            {
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
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return RedirectToAction(nameof(Index));
                }

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
                return View(product);
            }
            catch (Exception ex)
            {
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
                    return this.HandleError(HttpStatusCode.NotFound, "Product not found");
                }

                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return View(product);
            }
            catch (Exception ex)
            {
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
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return RedirectToAction(nameof(Index));
                }

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                ViewBag.Categories = new SelectList(_productService.GetCategories(), "Id", "Name", product.CategoryId);
                return View(product);
            }
            catch (Exception ex)
            {
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
                    return this.HandleError(HttpStatusCode.NotFound, "Product not found");
                }

                Response.StatusCode = (int)HttpStatusCode.OK;
                return View(product);
            }
            catch (Exception ex)
            {
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
                Response.StatusCode = (int)HttpStatusCode.OK;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error Deleting Product: {ex.Message}");
            }
        }

       
       
    }
}
