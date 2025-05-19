using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel;

namespace DigitalGameStore.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public ICollection<Licence>? Licences { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
