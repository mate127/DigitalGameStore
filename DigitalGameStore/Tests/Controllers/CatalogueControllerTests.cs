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
                .UseInMemoryDatabase("CatalogueTestDb_" + System.Guid.NewGuid())
                .Options;

            var context = new AppDbContext(options);
            context.Catalogues.AddRange(
                new Catalogue { CatalogueId = 1, Name = "Winter Sale" },
                new Catalogue { CatalogueId = 2, Name = "Summer Deals" }
            );
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithCatalogueList()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Catalogue>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ValidId_ReturnsCatalogue()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);

            var result = await controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Catalogue>(viewResult.Model);
            Assert.Equal(1, model.CatalogueId);
        }

        [Fact]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);

            var result = await controller.Details(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidModel_RedirectsToIndex()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);
            var newCatalogue = new Catalogue { CatalogueId = 3, Name = "Spring Discounts" };

            var result = await controller.Create(newCatalogue);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal(3, context.Catalogues.Count());
        }

        [Fact]
        public async Task Create_InvalidModel_ReturnsViewWithModel()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);
            controller.ModelState.AddModelError("Name", "Required");

            var result = await controller.Create(new Catalogue {Name=""});

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Catalogue>(viewResult.Model);
        }

        [Fact]
        public async Task Edit_ValidId_UpdatesCatalogue()
        {
            var context = GetDbContext();

            var trackedEntity = context.Catalogues.Local.FirstOrDefault(e => e.CatalogueId == 1);
            if (trackedEntity != null)
                context.Entry(trackedEntity).State = EntityState.Detached;

            var controller = new CatalogueController(context);
            var updatedCatalogue = new Catalogue { CatalogueId = 1, Name = "Winter Sale Updated", Description = "Updated desc" };

            var result = await controller.Edit(1, updatedCatalogue);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("Winter Sale Updated", context.Catalogues.Find(1)!.Name);
        }

        [Fact]
        public async Task Edit_InvalidId_ReturnsNotFound()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);
            var fakeCatalogue = new Catalogue { CatalogueId = 999, Name = "Fake" };

            var result = await controller.Edit(999, fakeCatalogue);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesCatalogue()
        {
            var context = GetDbContext();
            var controller = new CatalogueController(context);

            var result = await controller.DeleteConfirmed(1);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Null(context.Catalogues.Find(1));
        }
    }
}
