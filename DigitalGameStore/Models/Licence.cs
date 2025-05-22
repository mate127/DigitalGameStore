using System.ComponentModel.DataAnnotations;

namespace DigitalGameStore.Models
{
    public class Licence
    {
        //public int LicenceId { get; set; }

        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int GameId { get; set; }
        public Game? Game { get; set; }
    }
}
