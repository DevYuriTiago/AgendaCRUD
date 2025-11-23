using ContactAgenda.Domain.ValueObjects;
using FluentAssertions;

namespace ContactAgenda.Application.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Create_WithValidEmail_ShouldSucceed()
    {
        // Arrange
        var emailString = "test@example.com";

        // Act
        var email = Email.Create(emailString);

        // Assert
        email.Value.Should().Be("test@example.com");
    }

    [Fact]
    public void Create_WithValidEmail_ShouldNormalizeToLowercase()
    {
        // Arrange
        var emailString = "Test@EXAMPLE.COM";

        // Act
        var email = Email.Create(emailString);

        // Assert
        email.Value.Should().Be("test@example.com");
    }

    [Fact]
    public void Create_WithValidEmail_ShouldTrimWhitespace()
    {
        // Arrange
        var emailString = "  test@example.com  ";

        // Act
        var email = Email.Create(emailString);

        // Assert
        email.Value.Should().Be("test@example.com");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyEmail_ShouldThrowException(string? emailString)
    {
        // Act
        var act = () => Email.Create(emailString!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Email cannot be empty*");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test@@example.com")]
    [InlineData("test example@test.com")]
    public void Create_WithInvalidFormat_ShouldThrowException(string emailString)
    {
        // Act
        var act = () => Email.Create(emailString);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Invalid email format*");
    }

    [Fact]
    public void Create_WithTooLongEmail_ShouldThrowException()
    {
        // Arrange - email with 255 characters
        var emailString = new string('a', 240) + "@example.com";

        // Act
        var act = () => Email.Create(emailString);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Email is too long*");
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("TEST@EXAMPLE.COM");

        // Act & Assert
        email1.Should().Be(email2);
        (email1 == email2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var email1 = Email.Create("test1@example.com");
        var email2 = Email.Create("test2@example.com");

        // Act & Assert
        email1.Should().NotBe(email2);
        (email1 != email2).Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        var email = Email.Create("test@example.com");

        // Act
        string emailString = email;

        // Assert
        emailString.Should().Be("test@example.com");
    }
}
