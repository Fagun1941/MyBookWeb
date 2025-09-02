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
        public IActionResult Index()
        {
            var categories = _categoryService.GetAll();
            return View(categories);
        }

        // Details by Id
        public IActionResult Details(int id)
        {
            var category = _categoryService.GetById(id);
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
                _categoryService.Add(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Edit GET
        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetById(id);
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
                _categoryService.Update(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // Delete
        public IActionResult Delete(int id)
        {
            _categoryService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
