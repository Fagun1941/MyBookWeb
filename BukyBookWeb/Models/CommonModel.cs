using System.Net;

namespace BukyBookWeb.Models
{
    public class CommonModel
    {
       
        public string? Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object? Data { get; set; }
    }
}
