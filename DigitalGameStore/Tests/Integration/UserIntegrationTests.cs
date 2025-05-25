using DigitalGameStore.Data;
using DigitalGameStore.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DigitalGameStore.Tests.Integration
{
    public class UserIntegrationTests
    {
        private async Task<AppDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"UserDb_{System.Guid.NewGuid()}")
                .Options;
            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task CanInsertUserIntoDatabase()
        {
            using var context = await GetDbContextAsync();
            var user = new User { Username = "player1", Email = "player1@mail.com", Password = "pass123" };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            Assert.Single(context.Users);
        }

        [Fact]
        public async Task CanRetrieveUserById()
        {
            using var context = await GetDbContextAsync();
            var user = new User { Username = "gamer1", Email = "gamer1@mail.com", Password = "pass123" };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var retrieved = await context.Users.FindAsync(user.UserId);
            Assert.Equal("gamer1", retrieved?.Username);
        }
    }
}
