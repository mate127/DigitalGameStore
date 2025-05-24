using DigitalGameStore.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace DigitalGameStore.Tests.Models
{
    public class UserModelTests
    {
        [Fact]
        public void Username_IsRequired_ValidationFailsIfEmpty()
        {
            var user = new User { Email = "test@example.com", Password = "password123" };
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(user, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("Username"));
        }

        [Fact]
        public void Email_MustBeValidAddress()
        {
            var user = new User { Username = "Test", Email = "invalidEmail", Password = "password123" };
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(user, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }
    }
}
