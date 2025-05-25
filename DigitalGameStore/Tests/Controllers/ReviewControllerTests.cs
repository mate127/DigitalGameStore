using DigitalGameStore.Controllers;
using DigitalGameStore.Data;
using DigitalGameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DigitalGameStore.Tests.Controllers
{
    public class ReviewControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDb_Review_" + System.Guid.NewGuid())
                .Options;
            var context = new AppDbContext(options);

            // Test podaci
            var user = new User { UserId = 1, Username = "testuser", Email = "test@mail.com", Password = "1234"};
            var game = new Game { GameId = 1, Name = "Test Game" };

            context.Users.Add(user);
            context.Games.Add(game);
            context.Reviews.Add(new Review { ReviewId = 1, Description = "Great game", GameId = 1, UserId = 1 });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Index_ReturnsViewWithReviews()
        {
            var context = GetDbContext();
            var controller = new ReviewController(context);

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Review>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ValidId_ReturnsReview()
        {
            var context = GetDbContext();
            var controller = new ReviewController(context);

            var result = await controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Review>(viewResult.Model);
            Assert.Equal("Great game", model.Description);
        }

        [Fact]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            var context = GetDbContext();
            var controller = new ReviewController(context);

            var result = await controller.Details(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidReview_RedirectsToReturnUrl()
        {
            var context = GetDbContext();
            var controller = new ReviewController(context);

            var review = new Review { Description = "New review", GameId = 1, UserId = 1 };
            var result = await controller.Create(review, "/Game/Details/1");

            var redirect = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/Game/Details/1", redirect.Url);
            Assert.Equal(2, context.Reviews.Count());
        }

        [Fact]
        public async Task Edit_ValidReview_UpdatesReview()
        {
            var context = GetDbContext();
            var controller = new ReviewController(context);

            var review = context.Reviews.First();
            review.Description = "Updated review";

            var result = await controller.Edit(review.ReviewId, review, "/Game/Details/1");

            var redirect = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/Game/Details/1", redirect.Url);
            Assert.Equal("Updated review", context.Reviews.First().Description);
        }

        [Fact]
        public async Task DeleteConfirmed_DeletesReview()
        {
            var context = GetDbContext();
            var controller = new ReviewController(context);

            var result = await controller.DeleteConfirmed(1, "/Game/Details/1");

            var redirect = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/Game/Details/1", redirect.Url);
            Assert.Empty(context.Reviews);
        }
    }
}
