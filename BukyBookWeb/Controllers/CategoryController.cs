using BukyBookWeb.Helpers;
using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Net;
using Serilog.Context;
using Microsoft.AspNetCore.Authorization;

namespace BukyBookWeb.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private readonly IStringLocalizer<CategoryController> _localizer;

        public CategoryController(
            ICategoryService categoryService,
            ILogger<CategoryController> logger,
            IStringLocalizer<CategoryController> localizer)
        {
            _categoryService = categoryService;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            try
            {
                int pageSize = 3;

                var categories = await _categoryService.GetAllCategoryAsync(search, page, pageSize);
                int totalCategories = await _categoryService.GetTotalCountAsync(search);

                ViewBag.PageNumber = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(totalCategories / (double)pageSize);
                ViewBag.Search = search;

                return View(categories);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error loading categories. CorrelationId={LogGuid}", logGuid);
                }

                return this.HandleError(HttpStatusCode.InternalServerError, $"Error loading categories. Tracking ID: {logGuid}");
            }
        }

        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["SuccessMessage"] = _localizer["CategoryCreated"].Value;
                    await _categoryService.AddCategoryAsync(category);
                    return RedirectToAction(nameof(Index));
                }

                return this.HandleError(HttpStatusCode.BadRequest, "Invalid category data");
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error creating category. CorrelationId={LogGuid}", logGuid);
                }

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error creating category. Tracking ID: {logGuid}");
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdCategoryAsync(id);
                if (category == null)
                {
                    return this.HandleError(HttpStatusCode.NotFound, "Category not found");
                }

                return View(category);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error loading category for edit. CorrelationId={LogGuid}", logGuid);
                }

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error editing category. Tracking ID: {logGuid}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryService.UpdateCategoryAsync(category);
                    return RedirectToAction(nameof(Index));
                }

                return this.HandleError(HttpStatusCode.BadRequest, "Invalid category data");
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error updating category. CorrelationId={LogGuid}", logGuid);
                }

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error updating category. Tracking ID: {logGuid}");
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _categoryService.GetByIdCategoryAsync(id);
                if (category == null)
                {
                    return this.HandleError(HttpStatusCode.NotFound, "Category not found");
                }

                return View(category);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error loading delete page. CorrelationId={LogGuid}", logGuid);
                }

                return this.HandleError(HttpStatusCode.InternalServerError, $"Error loading delete page. Tracking ID: {logGuid}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error deleting category. CorrelationId={LogGuid}", logGuid);
                }

                return this.HandleError(HttpStatusCode.InternalServerError, $"Error deleting category. Tracking ID: {logGuid}");
            }
        }
    }
}
