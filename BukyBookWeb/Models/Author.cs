using System.ComponentModel.DataAnnotations;

namespace BukyBookWeb.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Required]
        public required string AuthorName { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
