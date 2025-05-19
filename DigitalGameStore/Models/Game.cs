using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel;

namespace DigitalGameStore.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime PublicationDate { get; set; }
        public decimal Price { get; set; }

        public int GenreId { get; set; }
        public GameGenre? Genre { get; set; }

        public int PublisherId { get; set; }
        public Admin? Publisher { get; set; }

        public int LicenceId { get; set; }
        public Licence? Licence { get; set; }

        public ICollection<Review>? Reviews { get; set; }
    }
}
