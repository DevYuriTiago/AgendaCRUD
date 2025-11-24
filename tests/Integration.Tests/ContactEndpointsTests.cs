using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using ContactAgenda.Application.DTOs;

namespace ContactAgenda.Integration.Tests;

public class ContactEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ContactEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetAuthTokenAsync()
    {
        var registerRequest = new
        {
            Username = $"user{Guid.NewGuid():N}",
            Email = $"user{Guid.NewGuid():N}@example.com",
            Password = "Test@123",
            FullName = "Test User"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        return result!.AccessToken;
    }

    [Fact]
    public async Task GetContacts_WithoutAuth_ReturnsUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/contacts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetContacts_WithAuth_ReturnsSuccess()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/contacts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var contacts = await response.Content.ReadFromJsonAsync<List<ContactDto>>();
        contacts.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateContact_WithAuth_ReturnsCreated()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var newContact = new
        {
            Name = "John Doe",
            Email = "john@example.com",
            Phone = "+5511999999999"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/contacts", newContact);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var contact = await response.Content.ReadFromJsonAsync<ContactDto>();
        contact.Should().NotBeNull();
        contact!.Name.Should().Be("John Doe");
        contact.Email.Should().Be("john@example.com");
        contact.Phone.Should().Be("5511999999999"); // Phone is normalized without +
    }

    [Fact]
    public async Task CreateContact_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var invalidContact = new
        {
            Name = "Jane Doe",
            Email = "invalid-email",
            Phone = "+5511888888888"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/contacts", invalidContact);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateContact_WithAuth_ReturnsSuccess()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create a contact first
        var newContact = new
        {
            Name = "Update Test",
            Email = "update@example.com",
            Phone = "+5511777777777"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/contacts", newContact);
        var createdContact = await createResponse.Content.ReadFromJsonAsync<ContactDto>();

        // Update the contact
        var updateContact = new
        {
            Id = createdContact!.Id,
            Name = "Updated Name",
            Email = "updated@example.com",
            Phone = "+5511666666666"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/contacts/{createdContact.Id}", updateContact);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<ContactDto>();
        updated.Should().NotBeNull();
        updated!.Name.Should().Be("Updated Name");
        updated.Email.Should().Be("updated@example.com");
    }

    [Fact]
    public async Task DeleteContact_WithAuth_ReturnsNoContent()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Create a contact first
        var newContact = new
        {
            Name = "Delete Test",
            Email = "delete@example.com",
            Phone = "+5511555555555"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/contacts", newContact);
        var createdContact = await createResponse.Content.ReadFromJsonAsync<ContactDto>();

        // Act
        var response = await _client.DeleteAsync($"/api/contacts/{createdContact!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify deletion
        var getResponse = await _client.GetAsync($"/api/contacts/{createdContact.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
