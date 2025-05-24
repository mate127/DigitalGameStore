using DigitalGameStore.Controllers;
using DigitalGameStore.Data;
using DigitalGameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DigitalGameStore.Tests.Integration
{
    public class GameIntegrationTests
    {
        [Fact]
        public async Task Create_ThenRead_Game()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("GameTestDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var controller = new GameController(context);

                var newGame = new Game
                {
                    Name = "Integracija",
                    Description = "Test integracije",
                    PublicationDate = DateTime.Now,
                    Price = 29.99m,
                    GenreId = 1,
                    PublisherId = 1
                };

                var result = await controller.Create(newGame) as RedirectToActionResult;
                Assert.Equal("Index", result?.ActionName);
            }

            using (var context = new AppDbContext(options))
            {
                var game = context.Games.FirstOrDefault(g => g.Name == "Integracija");
                Assert.NotNull(game);
            }
        }
    }
}
