using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
                //TempData["ErrorMessage"] = $"Error loading categories: {ex.Message}";
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Loading Categories : {ex.Message}"
                };
                return View("Error",errorModel); // You can create a shared Error.cshtml view
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
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error creating category: {ex.Message}"
                };

                return View("Error", errorModel);
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
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error category: {ex.Message}"
                };

                return View("Error", errorModel);
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
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Editing category: {ex.Message}"
                };

                return View("Error", errorModel);
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
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Editing category: {ex.Message}"
                };

                return View("Error", errorModel);
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
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Delete category: {ex.Message}"
                };

                return View("Error", errorModel);
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
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = $"Error Delete category: {ex.Message}"
                };

                return View("Error", errorModel);
            }

        }
    }
}
