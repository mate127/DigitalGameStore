using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DigitalGameStore.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "CheckEmail", controller: "User")]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        public ICollection<Licence>? Licences { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
