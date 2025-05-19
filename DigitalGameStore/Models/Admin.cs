namespace DigitalGameStore.Models
{
    public class Admin : User
    {
        public decimal Salary { get; set; }

        public ICollection<Game>? CreatedGames { get; set; }
    }
}
