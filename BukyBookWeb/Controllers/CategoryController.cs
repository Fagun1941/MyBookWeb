using BukyBookWeb.Helpers;
using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BukyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger; // ✅ add logger

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public IActionResult Index(string? search, int page = 1)
        {
            try
            {
                int pageSize = 3;
                var categories = _categoryService.GetAllCategory(search, page, pageSize);

                int totalCategories = _categoryService.GetTotalCount(search);
                ViewBag.PageNumber = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(totalCategories / (double)pageSize);
                ViewBag.Search = search;

                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading categories"); // ✅ log it
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error loading categories: {ex.Message}");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _categoryService.AddCategory(category);
                    return RedirectToAction(nameof(Index));
                }

                return this.HandleError(HttpStatusCode.BadRequest, "Invalid category data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category"); // ✅ log it
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error creating category: {ex.Message}");
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var category = _categoryService.GetByIdCategory(id);
                if (category == null)
                {
                    return this.HandleError(HttpStatusCode.NotFound, "Category not found");
                }

                return View(category); // ✅ should return category model to view
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading category for edit"); // ✅ log it
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error editing category: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _categoryService.UpdateCategory(category);
                    return RedirectToAction(nameof(Index));
                }

                return this.HandleError(HttpStatusCode.BadRequest, "Invalid category data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category"); // ✅ log it
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error updating category: {ex.Message}");
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var category = _categoryService.GetByIdCategory(id);
                if (category == null)
                {
                    return this.HandleError(HttpStatusCode.NotFound, "Category not found");
                }

                return View(category); // ✅ pass category to view
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete page"); // ✅ log it
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error loading delete page: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            try
            {
                _categoryService.DeleteCategory(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category"); // ✅ log it
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error deleting category: {ex.Message}");
            }
        }
    }
}
