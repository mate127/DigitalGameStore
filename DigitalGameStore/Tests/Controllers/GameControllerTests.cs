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
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GameTestDb_" + System.Guid.NewGuid()) // koristi GUID da izbjegne duplikate
                .Options;

            var context = new AppDbContext(options);

            // Dodaj potrebne entitete ako nisu već dodani
            if (!context.GameGenres.Any())
            {
                context.GameGenres.Add(new GameGenre { GenreId = 1, Name = "Action" });
            }

            if (!context.Admins.Any())
            {
                context.Admins.Add(new Admin { UserId = 1, Username = "Admin1", Email = "admin@example.com", Password = "pass123", Salary = 1000 });
            }

            if (!context.Games.Any())
            {
                context.Games.Add(new Game
                {
                    GameId = 1,
                    Name = "Test Game",
                    Description = "Test Description",
                    GenreId = 1,
                    PublisherId = 1,
                    Price = 49.99M,
                    PublicationDate = System.DateTime.Today
                });
            }

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfGames()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new GameController(context);

            // Act
            var result = await controller.Index("");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Game>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new GameController(context);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenGameDoesNotExist()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new GameController(context);

            // Act
            var result = await controller.Details(999); // id koji ne postoji

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithGame()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new GameController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Game>(viewResult.Model);
            Assert.Equal("Test Game", model.Name);
        }

        [Fact]
        public void Create_Get_ReturnsViewResult_WithViewData()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new GameController(context);

            // Act
            var result = controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(viewResult.ViewData.ContainsKey("GenreId"));
            Assert.True(viewResult.ViewData.ContainsKey("PublisherId"));
        }

        [Fact]
        public async Task Create_Post_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new GameController(context);

            var newGame = new Game
            {
                Name = "New Game",
                Description = "Description",
                GenreId = 1,
                PublisherId = 1,
                Price = 59.99m,
                PublicationDate = DateTime.Today
            };

            // Act
            var result = await controller.Create(newGame);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            Assert.True(context.Games.Any(g => g.Name == "New Game"));
        }

        [Fact]
        public async Task Edit_Get_ReturnsViewResult_WithGame()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new GameController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Game>(viewResult.Model);
            Assert.Equal(1, model.GameId);
        }

        [Fact]
        public async Task Edit_Post_UpdatesGame_WhenModelIsValid()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new GameController(context);

            var gameToUpdate = context.Games.First();
            gameToUpdate.Name = "Updated Game";

            // Act
            var result = await controller.Edit(gameToUpdate.GameId, gameToUpdate);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var updatedGame = context.Games.Find(gameToUpdate.GameId);
            Assert.Equal("Updated Game", updatedGame.Name);
        }

        [Fact]
        public async Task Delete_Get_ReturnsViewResult_WithGame()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new GameController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Game>(viewResult.Model);
            Assert.Equal(1, model.GameId);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesGameAndRedirects()
        {
            // Arrange
            var context = GetInMemoryContext();
            var controller = new GameController(context);

            // Act
            var result = await controller.DeleteConfirmed(1);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var game = context.Games.Find(1);
            Assert.Null(game);
        }
    }
}
