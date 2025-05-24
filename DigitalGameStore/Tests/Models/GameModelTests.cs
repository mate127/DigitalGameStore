using DigitalGameStore.Models;
using NuGet.ContentModel;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace DigitalGameStore.Tests.Models
{
    public class GameModelTests
    {
        [Fact]
        public void Game_WithInvalidName_ShouldFailValidation()
        {
            var game = new Game
            {
                Name = "",
                Description = "Opis",
                PublicationDate = DateTime.Now,
                Price = 49.99m,
                GenreId = 1,
                PublisherId = 1
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(game, null, null);
            bool isValid = Validator.TryValidateObject(game, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("Name"));
        }
    }
}
