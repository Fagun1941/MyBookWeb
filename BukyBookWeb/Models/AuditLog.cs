using System.ComponentModel.DataAnnotations;

namespace BukyBookWeb.Models
{
    public class AuditLog
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public required String EnityName { get; set; }
        [Required]
        public required int EntityId { get; set; }
        [Required]
        public required string Action { get; set; }
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required DateTime TimeAction { get; set; }  

        public String? ChangeDetails { get; set; }

    }
}
