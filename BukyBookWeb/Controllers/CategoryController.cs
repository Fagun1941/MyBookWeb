using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace BukyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Show all categories
        public IActionResult Index(string search)
        {
            ViewData["CurrentFilter"] = search ?? "";
            var categories = _categoryService.GetAllCategory(search);
            return View(categories);
        }

        // Details by Id
        public IActionResult Details(int id)
        {
            var category = _categoryService.GetByIdCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // Add GET
        public IActionResult Create()
        {
            return View();
        }

        // Add POST
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.AddCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Edit GET
        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetByIdCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // Edit POST
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.UpdateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Delete
        public IActionResult Delete(int id)
        {
            _categoryService.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
