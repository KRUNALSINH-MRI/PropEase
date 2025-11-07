using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropEase.Models
{
    public class PropertyMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ✅ ensures DB auto-generates ID
        public int Id { get; set; }

        public int PropertyId { get; set; }
        public string? PropertyTitle { get; set; }
        public string? OwnerId { get; set; }
        public string? SenderId { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime SentOn { get; set; } = DateTime.Now;
    }
}
