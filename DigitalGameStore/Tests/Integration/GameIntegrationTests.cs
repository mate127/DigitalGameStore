using DigitalGameStore.Controllers;
using DigitalGameStore.Data;
using DigitalGameStore.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DigitalGameStore.Tests.Integration
{
    public class GameIntegrationTests
    {
        private async Task<AppDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"GameDb_{System.Guid.NewGuid()}")
                .Options;
            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();

            context.GameGenres.Add(new GameGenre { GenreId = 1, Name = "Action" });
            context.Admins.Add(new Admin { UserId = 1, Username = "Epic", Email = "epic@mail.com", Password = "pass123" });
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task CanInsertGameIntoDatabase()
        {
            using var context = await GetDbContextAsync();
            var game = new Game
            {
                Name = "Super Game",
                Price = 49.99m,
                GenreId = 1,
                PublisherId = 1
            };

            context.Games.Add(game);
            await context.SaveChangesAsync();

            Assert.Single(context.Games);
        }

        [Fact]
        public async Task CanRetrieveGameWithRelations()
        {
            using var context = await GetDbContextAsync();
            var game = new Game
            {
                Name = "Mega Game",
                Price = 29.99m,
                GenreId = 1,
                PublisherId = 1
            };

            context.Games.Add(game);
            await context.SaveChangesAsync();

            var result = await context.Games
                .Include(g => g.Genre)
                .Include(g => g.Publisher)
                .FirstOrDefaultAsync();

            Assert.Equal("Action", result?.Genre.Name);
            Assert.Equal("Epic", result?.Publisher.Username);
        }
    }
}
