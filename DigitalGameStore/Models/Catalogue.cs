namespace DigitalGameStore.Models
{
    public class Catalogue
    {
        public int CatalogueId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<Game>? Games { get; set; }
    }
}
