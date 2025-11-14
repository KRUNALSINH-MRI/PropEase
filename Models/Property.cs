using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PropEase.Models
{
    public class Property
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // 1. Basic Details
        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public string PropertyType { get; set; }

        // 2. Location Info
        [Required] public string Address { get; set; }
        [Required] public string City { get; set; }
        [Required] public string State { get; set; }
        [Required, StringLength(10)] public string ZipCode { get; set; }

        // 3. Property Specifications
        [Range(0, 20)] public int BHK { get; set; }
        [Range(100, 100000)] public int Area { get; set; }
        [Display(Name = "Furnish Type")] public string FurnishType { get; set; }

        // 4. Pricing
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public string Type { get; set; } // Rent or Sale

        // 5. Media
        [Display(Name = "Property Image")]
        public string? ImageUrl { get; set; }

        // 6. Metadata

        // 🔗 Relationship to User (Owner)
        public string? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public ApplicationUser? Owner { get; set; }

        // Date Added
        [Display(Name = "Listed On")]
        public DateTime PostedOn { get; set; } = DateTime.Now;
 
    }
}
