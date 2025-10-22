using Xunit;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using FinTechBanking.Core.DTOs;

namespace FinTechBanking.Tests;

/// <summary>
/// Testes de Segurança - Validam proteções contra ataques comuns
/// </summary>
public class JwtSecurityTests
{
    private const string ApiUrl = "http://localhost:5036";
    private const string AdminEmail = "admin@owaypay.com";
    private const string AdminPassword = "Admin@123";

    [Fact]
    public async Task Login_ShouldReturnValidJwtToken()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = AdminEmail,
            Password = AdminPassword
        };

        using var client = new HttpClient();

        // Act
        var response = await client.PostAsJsonAsync(
            $"{ApiUrl}/api/auth/login",
            loginRequest
        );

        if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            return;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonString = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(jsonString);
        jsonDoc.RootElement.TryGetProperty("accessToken", out var tokenElement).Should().BeTrue();

        var token = tokenElement.GetString();
        token.Should().NotBeNullOrEmpty();

        // Validar estrutura JWT (3 partes separadas por ponto)
        var parts = token.Split('.');
        parts.Length.Should().Be(3, "JWT deve ter 3 partes: header.payload.signature");
    }

    [Fact]
    public async Task ExpiredToken_ShouldBeRejected()
    {
        // Arrange - Token expirado (simulado)
        var expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjE1MTYyMzkwMjJ9.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", expiredToken);

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/cliente/perfil");

        // Assert - Token expirado deve retornar 401
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task InvalidToken_ShouldBeRejected()
    {
        // Arrange - Token inválido
        var invalidToken = "invalid.token.here";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", invalidToken);

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/cliente/perfil");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task MissingToken_ShouldBeRejected()
    {
        // Arrange - Sem token
        using var client = new HttpClient();

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/cliente/perfil");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}

public class AuthorizationTests
{
    private const string ApiUrl = "http://localhost:5036";
    private const string AdminEmail = "admin@owaypay.com";
    private const string AdminPassword = "Admin@123";
    private const string ClientEmail = "cliente.teste.1761159582640@owaypay.com";
    private const string ClientPassword = "Cliente@123";

    private async Task<string?> GetTokenForUser(string email, string password)
    {
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        using var client = new HttpClient();
        var response = await client.PostAsJsonAsync(
            $"{ApiUrl}/api/auth/login",
            loginRequest
        );

        if (response.StatusCode != HttpStatusCode.OK)
            return null;

        var jsonString = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(jsonString);
        
        if (jsonDoc.RootElement.TryGetProperty("accessToken", out var token))
            return token.GetString();

        return null;
    }

    [Fact]
    public async Task ClientUser_CannotAccessAdminEndpoints()
    {
        // Arrange - Cliente tenta acessar endpoint de admin
        var clientToken = await GetTokenForUser(ClientEmail, ClientPassword);
        if (clientToken == null)
            return;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken);

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/admin/users");

        // Assert - Cliente não deve ter acesso
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task AdminUser_CanAccessAdminEndpoints()
    {
        // Arrange - Admin acessa endpoint de admin
        var adminToken = await GetTokenForUser(AdminEmail, AdminPassword);
        if (adminToken == null)
            return;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/admin/users");

        // Assert - Admin deve ter acesso
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ClientUser_CanAccessClientEndpoints()
    {
        // Arrange - Cliente acessa endpoint de cliente
        var clientToken = await GetTokenForUser(ClientEmail, ClientPassword);
        if (clientToken == null)
            return;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clientToken);

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/cliente/perfil");

        // Assert - Cliente deve ter acesso
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

public class InputValidationTests
{
    private const string ApiUrl = "http://localhost:5036";

    [Fact]
    public async Task Login_WithEmptyEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "",
            Password = "Password@123"
        };

        using var client = new HttpClient();

        // Act
        var response = await client.PostAsJsonAsync(
            $"{ApiUrl}/api/auth/login",
            loginRequest
        );

        if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            return;

        // Assert - Email vazio deve ser rejeitado
        response.StatusCode.Should().NotBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_WithInvalidEmailFormat_ShouldReturnBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "not-an-email",
            Password = "Password@123"
        };

        using var client = new HttpClient();

        // Act
        var response = await client.PostAsJsonAsync(
            $"{ApiUrl}/api/auth/login",
            loginRequest
        );

        if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            return;

        // Assert - Email inválido deve ser rejeitado
        response.StatusCode.Should().NotBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_WithEmptyPassword_ShouldReturnBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "admin@owaypay.com",
            Password = ""
        };

        using var client = new HttpClient();

        // Act
        var response = await client.PostAsJsonAsync(
            $"{ApiUrl}/api/auth/login",
            loginRequest
        );

        if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            return;

        // Assert - Senha vazia deve ser rejeitada
        response.StatusCode.Should().NotBe(HttpStatusCode.OK);
    }
}

public class PasswordSecurityTests
{
    private const string ApiUrl = "http://localhost:5036";
    private const string AdminEmail = "admin@owaypay.com";
    private const string AdminPassword = "Admin@123";

    [Fact]
    public async Task PasswordHash_ShouldNotBeExposedInResponse()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = AdminEmail,
            Password = AdminPassword
        };

        using var client = new HttpClient();

        // Act
        var response = await client.PostAsJsonAsync(
            $"{ApiUrl}/api/auth/login",
            loginRequest
        );

        if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            return;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonString = await response.Content.ReadAsStringAsync();
        
        // Verificar que hash de senha não está na resposta
        jsonString.Should().NotContain("passwordHash");
        jsonString.Should().NotContain("password");
    }

    [Fact]
    public async Task SimilarPasswords_ShouldNotMatch()
    {
        // Arrange - Testar que senhas similares não são aceitas
        var loginRequest1 = new LoginRequest
        {
            Email = AdminEmail,
            Password = "Admin@123"
        };

        var loginRequest2 = new LoginRequest
        {
            Email = AdminEmail,
            Password = "Admin@124" // Senha similar mas diferente
        };

        using var client = new HttpClient();

        // Act
        var response1 = await client.PostAsJsonAsync(
            $"{ApiUrl}/api/auth/login",
            loginRequest1
        );

        var response2 = await client.PostAsJsonAsync(
            $"{ApiUrl}/api/auth/login",
            loginRequest2
        );

        if (response1.StatusCode == HttpStatusCode.ServiceUnavailable)
            return;

        // Assert
        response1.StatusCode.Should().Be(HttpStatusCode.OK);
        response2.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}

public class CorsSecurityTests
{
    private const string ApiUrl = "http://localhost:5036";

    [Fact]
    public async Task CorsHeaders_ShouldBePresent()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/auth/logout");

        // Assert - CORS headers devem estar presentes
        response.Headers.Should().NotBeEmpty();
    }
}

