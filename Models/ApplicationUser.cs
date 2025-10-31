using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PropEase.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        
    }
}
