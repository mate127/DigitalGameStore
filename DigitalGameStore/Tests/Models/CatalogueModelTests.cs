using DigitalGameStore.Models;
using Xunit;

namespace DigitalGameStore.Tests.Models
{
    public class CatalogueModelTests
    {
        [Fact]
        public void Catalogue_CanSetAndGetProperties()
        {
            var catalogue = new Catalogue
            {
                CatalogueId = 1,
                Name = "Action",
                Description = "All action games"
            };

            Assert.Equal(1, catalogue.CatalogueId);
            Assert.Equal("Action", catalogue.Name);
            Assert.Equal("All action games", catalogue.Description);
        }

        [Fact]
        public void Catalogue_GamesCollection_IsInitializable()
        {
            var game = new Game { GameId = 1, Name = "Test Game" };
            var catalogue = new Catalogue
            {
                CatalogueId = 2,
                Name = "Adventure",
                Games = new List<Game> { game }
            };

            Assert.Single(catalogue.Games);
            Assert.Equal("Test Game", catalogue.Games.First().Name);
        }
    }
}
