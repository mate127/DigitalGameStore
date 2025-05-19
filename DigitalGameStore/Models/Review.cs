namespace DigitalGameStore.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string? Description { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int GameId { get; set; }
        public Game? Game { get; set; }
    }
}
