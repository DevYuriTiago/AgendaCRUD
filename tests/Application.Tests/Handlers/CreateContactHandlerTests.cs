using AutoMapper;
using ContactAgenda.Application.Commands;
using ContactAgenda.Application.DTOs;
using ContactAgenda.Application.Handlers;
using ContactAgenda.Domain.Entities;
using ContactAgenda.Domain.Exceptions;
using ContactAgenda.Domain.Repositories;
using ContactAgenda.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace ContactAgenda.Application.Tests.Handlers;

public class CreateContactHandlerTests
{
    private readonly Mock<IContactRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateContactHandler _handler;

    public CreateContactHandlerTests()
    {
        _repositoryMock = new Mock<IContactRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateContactHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldCreateContact()
    {
        // Arrange
        var command = new CreateContactCommand("John Doe", "john@example.com", "11987654321");
        var expectedDto = new ContactDto
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@example.com",
            Phone = "11987654321",
            CreatedAt = DateTime.UtcNow
        };

        _repositoryMock
            .Setup(r => r.EmailExistsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(m => m.Map<ContactDto>(It.IsAny<Contact>()))
            .Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("john@example.com");
        result.Phone.Should().Be("11987654321");

        _repositoryMock.Verify(
            r => r.EmailExistsAsync("john@example.com", null, It.IsAny<CancellationToken>()),
            Times.Once);

        _repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ShouldThrowDuplicateEmailException()
    {
        // Arrange
        var command = new CreateContactCommand("John Doe", "john@example.com", "11987654321");

        _repositoryMock
            .Setup(r => r.EmailExistsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DuplicateEmailException>()
            .WithMessage("*john@example.com*already exists*");

        _repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData("", "john@example.com", "11987654321")]
    [InlineData("Jo", "john@example.com", "11987654321")]
    public async Task Handle_WithInvalidName_ShouldThrowArgumentException(
        string name, string email, string phone)
    {
        // Arrange
        var command = new CreateContactCommand(name, email, phone);

        _repositoryMock
            .Setup(r => r.EmailExistsAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Theory]
    [InlineData("John Doe", "invalid-email", "11987654321")]
    [InlineData("John Doe", "", "11987654321")]
    public async Task Handle_WithInvalidEmail_ShouldThrowArgumentException(
        string name, string email, string phone)
    {
        // Arrange
        var command = new CreateContactCommand(name, email, phone);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Theory]
    [InlineData("John Doe", "john@example.com", "123")]
    [InlineData("John Doe", "john@example.com", "")]
    public async Task Handle_WithInvalidPhone_ShouldThrowArgumentException(
        string name, string email, string phone)
    {
        // Arrange
        var command = new CreateContactCommand(name, email, phone);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
