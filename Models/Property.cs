using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PropEase.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        [StringLength(500)]
        public required string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(100)]
        public required string Location { get; set; }

        [Display(Name = "Property Type")]
        public required string PropertyType { get; set; }

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        // 🔗 Relationship to User (Owner)
        [ForeignKey("Owner")]
        public string? OwnerId { get; set; }
        public IdentityUser? Owner { get; set; }

        // Date Added
        [Display(Name = "Listed On")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
