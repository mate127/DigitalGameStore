namespace DigitalGameStore.Models
{
    public class GameGenre
    {
        public int GameGenreId { get; set; }
        public required string Name { get; set; }

        public ICollection<Game>? Games { get; set; }
    }
}
