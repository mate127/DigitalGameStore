using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DigitalGameStore.Models
{
    public class Game
    {
        public int GameId { get; set; }

        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [CustomValidation(typeof(Game), nameof(ValidatePublicationDate))]
        public DateTime PublicationDate { get; set; }
        public decimal Price { get; set; }

        public int GenreId { get; set; }
        public GameGenre? Genre { get; set; }

        public int PublisherId { get; set; }
        public Admin? Publisher { get; set; }

        public ICollection<Licence>? Licences { get; set; }

        public ICollection<Review>? Reviews { get; set; }

        public ICollection<Catalogue>? Catalogues { get; set; }

        public static ValidationResult? ValidatePublicationDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Today)
            {
                return new ValidationResult("Datum objave ne može biti u budućnosti.");
            }
            return ValidationResult.Success;
        }
    }
}
