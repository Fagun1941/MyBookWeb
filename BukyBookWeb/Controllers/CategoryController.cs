using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace BukyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index(string search, int page = 1)
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
                // Log the exception here
                TempData["ErrorMessage"] = $"Error loading categories: {ex.Message}";
                return View("Error"); // You can create a shared Error.cshtml view
            }
        }

        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading create form: {ex.Message}";
                return View("Error");
            }
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
                return View(category);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating category: {ex.Message}";
                return View("Error");
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var category = _categoryService.GetByIdCategory(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading category for edit: {ex.Message}";
                return View("Error");
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
                return View(category);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating category: {ex.Message}";
                return View("Error");
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var category = _categoryService.GetByIdCategory(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading category for delete: {ex.Message}";
                return View("Error");
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
                TempData["ErrorMessage"] = $"Error deleting category: {ex.Message}";
                return View("Error");
            }
        }
    }
}
