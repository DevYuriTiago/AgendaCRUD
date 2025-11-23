using ContactAgenda.Domain.ValueObjects;
using FluentAssertions;

namespace ContactAgenda.Application.Tests.ValueObjects;

public class PhoneNumberTests
{
    [Fact]
    public void Create_WithValidPhone_ShouldSucceed()
    {
        // Arrange
        var phoneString = "11987654321";

        // Act
        var phone = PhoneNumber.Create(phoneString);

        // Assert
        phone.Value.Should().Be("11987654321");
    }

    [Theory]
    [InlineData("(11) 98765-4321", "11987654321")]
    [InlineData("+55 11 98765-4321", "5511987654321")]
    [InlineData("11 9 8765-4321", "11987654321")]
    [InlineData("(11)98765-4321", "11987654321")]
    public void Create_WithFormattedPhone_ShouldNormalizeToDigitsOnly(string input, string expected)
    {
        // Act
        var phone = PhoneNumber.Create(input);

        // Assert
        phone.Value.Should().Be(expected);
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrimAndNormalize()
    {
        // Arrange
        var phoneString = "  11987654321  ";

        // Act
        var phone = PhoneNumber.Create(phoneString);

        // Assert
        phone.Value.Should().Be("11987654321");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyPhone_ShouldThrowException(string? phoneString)
    {
        // Act
        var act = () => PhoneNumber.Create(phoneString!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Phone number cannot be empty*");
    }

    [Fact]
    public void Create_WithNoDigits_ShouldThrowException()
    {
        // Arrange
        var phoneString = "---()";

        // Act
        var act = () => PhoneNumber.Create(phoneString);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*must contain at least one digit*");
    }

    [Theory]
    [InlineData("1234567")]
    [InlineData("123")]
    public void Create_WithTooShortPhone_ShouldThrowException(string phoneString)
    {
        // Act
        var act = () => PhoneNumber.Create(phoneString);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Phone number is too short*");
    }

    [Fact]
    public void Create_WithTooLongPhone_ShouldThrowException()
    {
        // Arrange - 16 digits
        var phoneString = "1234567890123456";

        // Act
        var act = () => PhoneNumber.Create(phoneString);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Phone number is too long*");
    }

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("11987654321");
        var phone2 = PhoneNumber.Create("(11) 98765-4321");

        // Act & Assert
        phone1.Should().Be(phone2);
        (phone1 == phone2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("11987654321");
        var phone2 = PhoneNumber.Create("11987654322");

        // Act & Assert
        phone1.Should().NotBe(phone2);
        (phone1 != phone2).Should().BeTrue();
    }

    [Fact]
    public void ImplicitConversion_ToString_ShouldReturnValue()
    {
        // Arrange
        var phone = PhoneNumber.Create("11987654321");

        // Act
        string phoneString = phone;

        // Assert
        phoneString.Should().Be("11987654321");
    }
}
