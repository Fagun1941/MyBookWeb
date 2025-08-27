using System.ComponentModel.DataAnnotations;

namespace BukyBookWeb.Models
{
    public class MyTable
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}
