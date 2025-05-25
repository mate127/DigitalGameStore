using DigitalGameStore.Data;
using DigitalGameStore.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DigitalGameStore.Tests.Integration
{
    public class CatalogueIntegrationTests
    {
        private async Task<AppDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"CatalogueDb_{System.Guid.NewGuid()}")
                .Options;
            var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task CanInsertCatalogueIntoDatabase()
        {
            using var context = await GetDbContextAsync();
            var catalogue = new Catalogue { Name = "Top Games", Description = "Popular titles" };

            context.Catalogues.Add(catalogue);
            await context.SaveChangesAsync();

            Assert.Single(context.Catalogues);
        }

        [Fact]
        public async Task CanRetrieveCatalogueById()
        {
            using var context = await GetDbContextAsync();
            var catalogue = new Catalogue { Name = "Indie", Description = "Indie games" };

            context.Catalogues.Add(catalogue);
            await context.SaveChangesAsync();

            var retrieved = await context.Catalogues.FindAsync(catalogue.CatalogueId);
            Assert.Equal("Indie", retrieved?.Name);
        }
    }
}
