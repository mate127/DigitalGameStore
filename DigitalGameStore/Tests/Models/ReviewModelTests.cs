using DigitalGameStore.Models;
using Xunit;

namespace DigitalGameStore.Tests.Models
{
    public class ReviewModelTests
    {
        [Fact]
        public void Review_CanSetAndGetProperties()
        {
            var review = new Review
            {
                ReviewId = 1,
                Description = "Amazing game",
                GameId = 10,
                UserId = 5
            };

            Assert.Equal(1, review.ReviewId);
            Assert.Equal("Amazing game", review.Description);
            Assert.Equal(10, review.GameId);
            Assert.Equal(5, review.UserId);
        }

        [Fact]
        public void Review_CanSetNavigationProperties()
        {
            var game = new Game { GameId = 10, Name = "Zelda" };
            var user = new User { UserId = 5, Username = "player1", Email = "test@mail.com", Password = "pass123" };

            var review = new Review
            {
                ReviewId = 2,
                Game = game,
                User = user
            };

            Assert.Equal("Zelda", review.Game.Name);
            Assert.Equal("player1", review.User.Username);
        }
    }
}
