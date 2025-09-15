using BukyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace BukyBookWeb.Helpers
{
    public static class ErrorHelper
    {
        public static IActionResult HandleError(this Controller controller, HttpStatusCode statusCode, string message)
        {
            var errorModel = new CommonModel
            {
                Message = message,
                StatusCode = statusCode
            };
            
            controller.Response.StatusCode = (int)statusCode;
            return controller.View("Error", errorModel);
        }
    }
}
