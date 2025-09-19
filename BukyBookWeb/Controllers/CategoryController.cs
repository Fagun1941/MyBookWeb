using BukyBookWeb.Helpers;
using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Net;
using Serilog.Context;
using Microsoft.Extensions.Caching.Memory;


namespace BukyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private readonly IStringLocalizer<CategoryController> _localizer;
        private readonly IMemoryCache _cache;

        public CategoryController(
            ICategoryService categoryService,
            ILogger<CategoryController> logger,
            IStringLocalizer<CategoryController> localizer,
            IMemoryCache cache)
        {
            _categoryService = categoryService;
            _logger = logger;
            _localizer = localizer;
            _cache = cache;
        }

        public IActionResult Index(string? search, int page = 1)
        {
            try
            {
                int pageSize = 3;
                string cacheKey = $"CategoryList_{search}_{page}_{pageSize}";

                if (!_cache.TryGetValue(cacheKey, out IEnumerable<Category> categories))
                {
                    categories = _categoryService.GetAllCategory(search, page, pageSize);
                    int totalCategories = _categoryService.GetTotalCount(search);

                    ViewBag.PageNumber = page;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalPages = (int)Math.Ceiling(totalCategories / (double)pageSize);
                    ViewBag.Search = search;

                    // Store in cache (expire in 2 minutes, auto-remove if unused for 30 seconds)
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                    _cache.Set(cacheKey, categories, cacheOptions);

                    _logger.LogInformation("Stored categories in cache: {CacheKey}", cacheKey);
                }
                else
                {
                    _logger.LogInformation("Loaded categories from cache: {CacheKey}", cacheKey);

                    // You still need to recalc ViewBag values, even when data is cached
                    int totalCategories = _categoryService.GetTotalCount(search);
                    ViewBag.PageNumber = page;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalPages = (int)Math.Ceiling(totalCategories / (double)pageSize);
                    ViewBag.Search = search;
                }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClearCategoryCache();

                    TempData["SuccessMessage"] = _localizer["CategoryCreated"].Value;
                    _categoryService.AddCategory(category);
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

        public IActionResult Edit(int id)
        {
            try
            {
                var category = _categoryService.GetByIdCategory(id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   // ClearCategoryCache();


                    _categoryService.UpdateCategory(category);
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

        public IActionResult Delete(int id)
        {
            try
            {
                var category = _categoryService.GetByIdCategory(id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            try
            {
                ClearCategoryCache();

                _categoryService.DeleteCategory(id);
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
        private void ClearCategoryCache()
        {
            // In real-world apps, better use IMemoryCache with ICacheEntry tracking
            // but here we can clear everything
            (_cache as MemoryCache)?.Compact(1.0);
        }

    }
}
