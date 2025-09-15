using System.ComponentModel.DataAnnotations;

namespace BukyBookWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Range(1, 50, ErrorMessage = "Order must be between 1 and 50")]
        public int DisplayOrder { get; set; }

        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
