using System.ComponentModel.DataAnnotations;

namespace BukyBookWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Range(1,50,ErrorMessage = "Order Must be 1 between 50")]
        public required string DisplayOrder { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
