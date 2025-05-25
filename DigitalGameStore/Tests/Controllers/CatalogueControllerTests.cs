using DigitalGameStore.Controllers;
using DigitalGameStore.Data;
using DigitalGameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DigitalGameStore.Tests.Controllers
{
    public class CatalogueControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            context.Catalogues.Add(new Catalogue
            {
                CatalogueId = 1,
                Name = "Top Games",
                Description = "Best of the best"
            });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Index_ReturnsViewWithCatalogues()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);

            var result = await controller.Index(null) as ViewResult;
            var model = result?.Model as System.Collections.Generic.List<Catalogue>;

            Assert.NotNull(result);
            Assert.Single(model);
            Assert.Equal("Top Games", model.First().Name);
        }

        [Fact]
        public async Task Index_WithSearch_ReturnsFilteredCatalogues()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);

            var result = await controller.Index("Top") as ViewResult;
            var model = result?.Model as System.Collections.Generic.List<Catalogue>;

            Assert.NotNull(result);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ValidId_ReturnsViewWithCatalogue()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);

            var result = await controller.Details(1) as ViewResult;
            var model = result?.Model as Catalogue;

            Assert.NotNull(result);
            Assert.Equal(1, model.CatalogueId);
        }

        [Fact]
        public async Task Create_PostValidModel_RedirectsToIndex()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);

            var newCatalogue = new Catalogue
            {
                Name = "New Collection",
                Description = "Brand new"
            };

            var result = await controller.Create(newCatalogue) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal(2, context.Catalogues.Count());
        }

        [Fact]
        public async Task Edit_PostValidModel_UpdatesCatalogue()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);

            var catalogue = context.Catalogues.First();
            catalogue.Name = "Updated Name";

            var result = await controller.Edit(catalogue.CatalogueId, catalogue) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Updated Name", context.Catalogues.First().Name);
        }

        [Fact]
        public async Task DeleteConfirmed_ValidId_DeletesCatalogue()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);

            var result = await controller.DeleteConfirmed(1) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Empty(context.Catalogues);
        }
    }
}
