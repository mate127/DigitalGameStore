using DigitalGameStore.Data;
using DigitalGameStore.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DigitalGameStore.Tests.Integration
{
    public class ReviewIntegrationTests
    {
        private async Task<AppDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"ReviewDb_{System.Guid.NewGuid()}")
                .Options;
            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();

            // Seed game and user
            context.Games.Add(new Game { GameId = 1, Name = "Test Game", Price = 10 });
            context.Users.Add(new User { UserId = 1, Username = "testuser", Email = "test@mail.com", Password = "pass123" });
            await context.SaveChangesAsync();

            return context;
        }

        [Fact]
        public async Task CanInsertReviewIntoDatabase()
        {
            using var context = await GetDbContextAsync();
            var review = new Review
            {
                Description = "Great game!",
                GameId = 1,
                UserId = 1
            };

            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            Assert.Single(context.Reviews);
        }

        [Fact]
        public async Task CanRetrieveReviewWithRelations()
        {
            using var context = await GetDbContextAsync();
            var review = new Review
            {
                Description = "Excellent!",
                GameId = 1,
                UserId = 1
            };

            context.Reviews.Add(review);
            await context.SaveChangesAsync();

            var result = await context.Reviews
                .Include(r => r.Game)
                .Include(r => r.User)
                .FirstOrDefaultAsync();

            Assert.Equal("Test Game", result?.Game.Name);
            Assert.Equal("testuser", result?.User.Username);
        }
    }
}
