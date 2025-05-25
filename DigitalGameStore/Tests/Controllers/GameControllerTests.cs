using DigitalGameStore.Controllers;
using DigitalGameStore.Data;
using DigitalGameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DigitalGameStore.Tests.Controllers
{
    public class GameControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            context.GameGenres.Add(new GameGenre { GenreId = 1, Name = "RPG" });
            context.Admins.Add(new Admin { UserId = 1, Username = "admin", Email = "admin@mail.com", Password = "pass123" });

            context.Games.Add(new Game
            {
                GameId = 1,
                Name = "Skyrim",
                Description = "Epic RPG",
                Price = 59.99m,
                PublicationDate = new DateTime(2011, 11, 11),
                GenreId = 1,
                PublisherId = 1
            });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Index_ReturnsViewWithAllGames()
        {
            var context = GetDbContext();
            var controller = new GameController(context);

            var result = await controller.Index(null) as ViewResult;
            var model = result?.Model as System.Collections.Generic.List<Game>;

            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Index_WithSearch_ReturnsFilteredGames()
        {
            var context = GetDbContext();
            var controller = new GameController(context);

            var result = await controller.Index("Skyrim") as ViewResult;
            var model = result?.Model as System.Collections.Generic.List<Game>;

            Assert.NotNull(result);
            Assert.Single(model);
            Assert.Equal("Skyrim", model.First().Name);
        }

        [Fact]
        public async Task Details_ValidId_ReturnsView()
        {
            var context = GetDbContext();
            var controller = new GameController(context);

            var result = await controller.Details(1) as ViewResult;
            var model = result?.Model as Game;

            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(1, model.GameId);
        }

        [Fact]
        public async Task Create_PostValidGame_RedirectsToIndex()
        {
            var context = GetDbContext();
            var controller = new GameController(context);

            var newGame = new Game
            {
                Name = "Portal",
                Description = "Puzzle game",
                Price = 19.99m,
                PublicationDate = new DateTime(2007, 10, 10),
                GenreId = 1,
                PublisherId = 1
            };

            var result = await controller.Create(newGame) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal(2, context.Games.Count()); // 1 existing + 1 new
        }

        [Fact]
        public async Task Edit_PostValidGame_UpdatesGame()
        {
            var context = GetDbContext();
            var controller = new GameController(context);

            var updatedGame = context.Games.First();
            updatedGame.Name = "Skyrim Remastered";

            var result = await controller.Edit(updatedGame.GameId, updatedGame) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Skyrim Remastered", context.Games.First().Name);
        }

        [Fact]
        public async Task DeleteConfirmed_ValidId_DeletesGame()
        {
            var context = GetDbContext();
            var controller = new GameController(context);

            var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Empty(context.Games);
        }
    }
}
