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
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorController> _logger;
        private readonly IStringLocalizer<AuthorController> _localizer;

        public AuthorController(
            IAuthorService authorService,
            ILogger<AuthorController> logger,
            IStringLocalizer<AuthorController> localizer)
        {
            _authorService = authorService;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            try
            {
                int pageSize = 3;

                var authors = await _authorService.GetAllAuthorAsync(search, page, pageSize);
                int totalAuthors = await _authorService.GetTotalCountAsync(search);

                ViewBag.PageNumber = page;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling(totalAuthors / (double)pageSize);
                ViewBag.Search = search;

                return View(authors);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error loading authors. CorrelationId={LogGuid}", logGuid);
                }

                return this.HandleError(HttpStatusCode.InternalServerError, $"Error loading authors. Tracking ID: {logGuid}");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["SuccessMessage"] = _localizer["AuthorCreated"].Value;
                    await _authorService.AddAuthorAsync(author);
                    return RedirectToAction(nameof(Index));
                }

                return this.HandleError(HttpStatusCode.BadRequest, "Invalid author data");
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error creating author. CorrelationId={LogGuid}", logGuid);
                }

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error creating author. Tracking ID: {logGuid}");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var author = await _authorService.GetByIdAuthorAsync(id);
                if (author == null)
                {
                    return this.HandleError(HttpStatusCode.NotFound, "Author not found");
                }

                return View(author);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error loading author for edit. CorrelationId={LogGuid}", logGuid);
                }

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error editing author. Tracking ID: {logGuid}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Author author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _authorService.UpdateAuthorAsync(author);
                    return RedirectToAction(nameof(Index));
                }

                return this.HandleError(HttpStatusCode.BadRequest, "Invalid author data");
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error updating author. CorrelationId={LogGuid}", logGuid);
                }

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error updating author. Tracking ID: {logGuid}");
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var author = await _authorService.GetByIdAuthorAsync(id);
                if (author == null)
                {
                    return this.HandleError(HttpStatusCode.NotFound, "Author not found");
                }

                return View(author);
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
                await _authorService.DeleteAuthorAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error deleting author. CorrelationId={LogGuid}", logGuid);
                }

                return this.HandleError(HttpStatusCode.InternalServerError, $"Error deleting author. Tracking ID: {logGuid}");
            }
        }
    }
}
