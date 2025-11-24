using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ContactAgenda.Application.DTOs;

namespace ContactAgenda.Integration.Tests;

public class AuthEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var request = new
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "Test@123",
            FullName = "Test User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result.Should().NotBeNull();
        result!.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.Username.Should().Be("testuser");
        result.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task Register_WithDuplicateUsername_ReturnsBadRequest()
    {
        // Arrange
        var request = new
        {
            Username = "duplicateuser",
            Email = "user1@example.com",
            Password = "Test@123",
            FullName = "User One"
        };

        // Act - First registration
        await _client.PostAsJsonAsync("/api/auth/register", request);

        // Act - Second registration with same username
        var request2 = new
        {
            Username = "duplicateuser",
            Email = "user2@example.com",
            Password = "Test@123",
            FullName = "User Two"
        };
        var response = await _client.PostAsJsonAsync("/api/auth/register", request2);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsSuccess()
    {
        // Arrange - Register a user first
        var registerRequest = new
        {
            Username = "loginuser",
            Email = "login@example.com",
            Password = "Test@123",
            FullName = "Login User"
        };
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new
        {
            Username = "loginuser",
            Password = "Test@123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result.Should().NotBeNull();
        result!.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        // Arrange - Register a user first
        var registerRequest = new
        {
            Username = "wrongpassuser",
            Email = "wrongpass@example.com",
            Password = "Test@123",
            FullName = "Wrong Pass User"
        };
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new
        {
            Username = "wrongpassuser",
            Password = "WrongPassword@123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshToken_WithValidToken_ReturnsNewTokens()
    {
        // Arrange - Register and get tokens
        var registerRequest = new
        {
            Username = "refreshuser",
            Email = "refresh@example.com",
            Password = "Test@123",
            FullName = "Refresh User"
        };
        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
        var authResult = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();

        var refreshRequest = new
        {
            RefreshToken = authResult!.RefreshToken
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/refresh", refreshRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result.Should().NotBeNull();
        result!.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBe(authResult.RefreshToken); // Should be a new token
    }

    [Fact]
    public async Task RefreshToken_WithInvalidToken_ReturnsUnauthorized()
    {
        // Arrange
        var refreshRequest = new
        {
            RefreshToken = "invalid-token-12345"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/refresh", refreshRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
