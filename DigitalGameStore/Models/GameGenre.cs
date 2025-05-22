using System.ComponentModel.DataAnnotations;

namespace DigitalGameStore.Models
{
    public class GameGenre
    {
        [Key]
        public int GenreId { get; set; }
        public required string Name { get; set; }

        public ICollection<Game>? Games { get; set; }
    }
}
