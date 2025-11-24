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

public class UpdateContactHandlerTests
{
    private readonly Mock<IContactRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateContactHandler _handler;

    public UpdateContactHandlerTests()
    {
        _repositoryMock = new Mock<IContactRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateContactHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidData_ShouldUpdateContact()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var command = new UpdateContactCommand(contactId, "Jane Doe", "jane@example.com", "11987654321");

        var existingContact = Contact.Create(
            "John Doe",
            Email.Create("john@example.com"),
            PhoneNumber.Create("11999999999"));

        var updatedDto = new ContactDto
        {
            Id = contactId,
            Name = "Jane Doe",
            Email = "jane@example.com",
            Phone = "11987654321",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingContact);

        _repositoryMock
            .Setup(r => r.EmailExistsAsync(It.IsAny<string>(), contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mapperMock
            .Setup(m => m.Map<ContactDto>(It.IsAny<Contact>()))
            .Returns(updatedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Jane Doe");
        result.Email.Should().Be("jane@example.com");
        result.Phone.Should().Be("11987654321");

        _repositoryMock.Verify(
            r => r.GetByIdAsync(contactId, It.IsAny<CancellationToken>()),
            Times.Once);

        _repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentContact_ShouldThrowContactNotFoundException()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var command = new UpdateContactCommand(contactId, "Jane Doe", "jane@example.com", "11987654321");

        _repositoryMock
            .Setup(r => r.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contact?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ContactNotFoundException>()
            .WithMessage($"*{contactId}*não foi encontrado*");

        _repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WithDuplicateEmail_ShouldThrowDuplicateEmailException()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var command = new UpdateContactCommand(contactId, "Jane Doe", "jane@example.com", "11987654321");

        var existingContact = Contact.Create(
            "John Doe",
            Email.Create("john@example.com"),
            PhoneNumber.Create("11999999999"));

        _repositoryMock
            .Setup(r => r.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingContact);

        _repositoryMock
            .Setup(r => r.EmailExistsAsync(It.IsAny<string>(), contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DuplicateEmailException>()
            .WithMessage("*jane@example.com*já existe*");

        _repositoryMock.Verify(
            r => r.UpdateAsync(It.IsAny<Contact>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Theory]
    [InlineData("", "jane@example.com", "11987654321")]
    [InlineData("Jo", "jane@example.com", "11987654321")]
    public async Task Handle_WithInvalidName_ShouldThrowArgumentException(
        string name, string email, string phone)
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var command = new UpdateContactCommand(contactId, name, email, phone);

        var existingContact = Contact.Create(
            "John Doe",
            Email.Create("john@example.com"),
            PhoneNumber.Create("11999999999"));

        _repositoryMock
            .Setup(r => r.GetByIdAsync(contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingContact);

        _repositoryMock
            .Setup(r => r.EmailExistsAsync(It.IsAny<string>(), contactId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
