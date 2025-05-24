using DigitalGameStore.Controllers;
using DigitalGameStore.Data;
using DigitalGameStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DigitalGameStore.Tests.Controllers
{
    public class UserControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UserTestDb_" + System.Guid.NewGuid())
                .Options;

            var context = new AppDbContext(options);

            context.Users.AddRange(
                new User { UserId = 1, Username = "test1", Email = "test1@example.com", Password = "pass123" },
                new User { UserId = 2, Username = "test2", Email = "test2@example.com", Password = "pass456" }
            );
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task Index_ReturnsViewWithUsers()
        {
            var context = GetInMemoryDbContext();
            var controller = new UserController(context);

            var result = await controller.Index() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(result.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsView_WhenUserExists()
        {
            var context = GetInMemoryDbContext();
            var controller = new UserController(context);

            var result = await controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<User>(viewResult.Model);
            Assert.Equal(1, model.UserId);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var context = GetInMemoryDbContext();
            var controller = new UserController(context);

            var result = await controller.Details(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_ValidUser_RedirectsToIndex()
        {
            var context = GetInMemoryDbContext();
            var controller = new UserController(context);

            var newUser = new User
            {
                Username = "test3",
                Email = "test3@example.com",
                Password = "password"
            };

            var result = await controller.Create(newUser);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal(3, context.Users.Count());
        }

        [Fact]
        public async Task Edit_Post_ValidData_UpdatesUser()
        {
            var context = GetInMemoryDbContext();
            var controller = new UserController(context);

            var existingUser = context.Users.First();
            existingUser.Username = "updated";

            var result = await controller.Edit(existingUser.UserId, existingUser);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            var updated = context.Users.Find(existingUser.UserId);
            Assert.Equal("updated", updated.Username);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesUser()
        {
            var context = GetInMemoryDbContext();
            var controller = new UserController(context);

            var result = await controller.DeleteConfirmed(1);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.DoesNotContain(context.Users, u => u.UserId == 1);
        }
    }
}
