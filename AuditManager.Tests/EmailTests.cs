using Bogus;
using Xunit;

namespace AuditManager.Tests
{
    public class EmailTests
    {
        [Fact]
        public void Create_returns_an_Email()
        {
            Result<Email> result = Email.Create("email@email.com");

            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<Email>(result.Value);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Create_returns_a_failure_for_empty_values(string emptyEmail)
        {
            Result<Email> result = Email.Create(emptyEmail);

            Assert.True(result.IsFailure);
            Assert.Equal("Email should not be empty", result.Error);
        }

        [Fact]
        public void Create_returns_a_failure_for_values_longer_than_256_characters()
        {
            var faker = new Faker();
            Result<Email> result = Email.Create(faker.Random.String(257));

            Assert.True(result.IsFailure);
            Assert.Equal("Email is too long", result.Error);
        }

        [Fact]
        public void Create_returns_a_failure_for_invalid_emails()
        {
            Result<Email> result = Email.Create("asdf");

            Assert.True(result.IsFailure);
            Assert.Equal("Email is invalid", result.Error);
        }

        [Fact]
        public void Emails_are_comparable()
        {
            Email email1 = (Email) "email@email.com";
            Email email2 = (Email) "email@email.com";

            Assert.True(email1.Equals(email2));
        }

        [Fact]
        public void Emails_are_comparable_by_equals_operator()
        {
            Email email1 = (Email) "email@email.com";
            Email email2 = (Email) "email@email.com";
            Email email3 = (Email) "asdf@email.com";

            Assert.True(email1 == email2);
            Assert.True(email1 != email3);
            Assert.True(email1 != null);
        }

        [Fact]
        public void Email_is_convertable_to_string()
        {
            Email email = Email.Create("email@email.com").Value;

            string emailAsString = email;

            Assert.Equal("email@email.com", emailAsString);
        }
    }
}
