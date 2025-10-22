using Xunit;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Diagnostics;
using FinTechBanking.Core.DTOs;

namespace FinTechBanking.Tests;

/// <summary>
/// Testes de Performance - Validam latência e throughput dos endpoints
/// </summary>
public class EndpointLatencyTests
{
    private const string ApiUrl = "http://localhost:5036";
    private const string AdminEmail = "admin@owaypay.com";
    private const string AdminPassword = "Admin@123";
    private const string ClientEmail = "cliente.teste.1761159582640@owaypay.com";
    private const string ClientPassword = "Cliente@123";
    private const int MaxLatencyMs = 500; // Máximo 500ms por requisição

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
    public async Task Login_ShouldCompleteWithinMaxLatency()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = AdminEmail,
            Password = AdminPassword
        };

        using var client = new HttpClient();
        var stopwatch = Stopwatch.StartNew();

        // Act
        var response = await client.PostAsJsonAsync(
            $"{ApiUrl}/api/auth/login",
            loginRequest
        );
        stopwatch.Stop();

        // Assert
        if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            return;

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(MaxLatencyMs,
            $"Login deve completar em menos de {MaxLatencyMs}ms, levou {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task GetSaldo_ShouldCompleteWithinMaxLatency()
    {
        // Arrange
        var token = await GetTokenForUser(ClientEmail, ClientPassword);
        if (token == null)
            return;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var stopwatch = Stopwatch.StartNew();

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/cliente/saldo");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(MaxLatencyMs,
            $"GetSaldo deve completar em menos de {MaxLatencyMs}ms, levou {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task GetTransacoes_ShouldCompleteWithinMaxLatency()
    {
        // Arrange
        var token = await GetTokenForUser(ClientEmail, ClientPassword);
        if (token == null)
            return;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var stopwatch = Stopwatch.StartNew();

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/cliente/transacoes");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(MaxLatencyMs,
            $"GetTransacoes deve completar em menos de {MaxLatencyMs}ms, levou {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task GetPerfil_ShouldCompleteWithinMaxLatency()
    {
        // Arrange
        var token = await GetTokenForUser(ClientEmail, ClientPassword);
        if (token == null)
            return;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var stopwatch = Stopwatch.StartNew();

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/cliente/perfil");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(MaxLatencyMs,
            $"GetPerfil deve completar em menos de {MaxLatencyMs}ms, levou {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task GetAdminUsers_ShouldCompleteWithinMaxLatency()
    {
        // Arrange
        var token = await GetTokenForUser(AdminEmail, AdminPassword);
        if (token == null)
            return;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var stopwatch = Stopwatch.StartNew();

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/admin/users");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(MaxLatencyMs,
            $"GetAdminUsers deve completar em menos de {MaxLatencyMs}ms, levou {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task GetAdminDashboard_ShouldCompleteWithinMaxLatency()
    {
        // Arrange
        var token = await GetTokenForUser(AdminEmail, AdminPassword);
        if (token == null)
            return;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var stopwatch = Stopwatch.StartNew();

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/admin/dashboard");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(MaxLatencyMs,
            $"GetAdminDashboard deve completar em menos de {MaxLatencyMs}ms, levou {stopwatch.ElapsedMilliseconds}ms");
    }
}

public class ConcurrentRequestTests
{
    private const string ApiUrl = "http://localhost:5036";
    private const string AdminEmail = "admin@owaypay.com";
    private const string AdminPassword = "Admin@123";

    [Fact]
    public async Task MultipleLoginRequests_ShouldHandleConcurrency()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = AdminEmail,
            Password = AdminPassword
        };

        var tasks = new List<Task<HttpResponseMessage>>();
        var concurrentRequests = 5;

        // Act
        for (int i = 0; i < concurrentRequests; i++)
        {
            var task = Task.Run(async () =>
            {
                using var client = new HttpClient();
                return await client.PostAsJsonAsync(
                    $"{ApiUrl}/api/auth/login",
                    loginRequest
                );
            });
            tasks.Add(task);
        }

        var responses = await Task.WhenAll(tasks);

        // Assert
        responses.Should().HaveCount(concurrentRequests);
        responses.Should().AllSatisfy(r => 
        {
            if (r.StatusCode != HttpStatusCode.ServiceUnavailable)
                r.StatusCode.Should().Be(HttpStatusCode.OK);
        });
    }

    [Fact]
    public async Task MultipleGetRequests_ShouldHandleConcurrency()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = AdminEmail,
            Password = AdminPassword
        };

        using var loginClient = new HttpClient();
        var loginResponse = await loginClient.PostAsJsonAsync(
            $"{ApiUrl}/api/auth/login",
            loginRequest
        );

        if (loginResponse.StatusCode != HttpStatusCode.OK)
            return;

        var jsonString = await loginResponse.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(jsonString);
        
        if (!jsonDoc.RootElement.TryGetProperty("accessToken", out var tokenElement))
            return;

        var token = tokenElement.GetString();

        var tasks = new List<Task<HttpResponseMessage>>();
        var concurrentRequests = 5;

        // Act
        for (int i = 0; i < concurrentRequests; i++)
        {
            var task = Task.Run(async () =>
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return await client.GetAsync($"{ApiUrl}/api/admin/users");
            });
            tasks.Add(task);
        }

        var responses = await Task.WhenAll(tasks);

        // Assert
        responses.Should().HaveCount(concurrentRequests);
        responses.Should().AllSatisfy(r => r.StatusCode.Should().Be(HttpStatusCode.OK));
    }
}

public class ResponseSizeTests
{
    private const string ApiUrl = "http://localhost:5036";
    private const string AdminEmail = "admin@owaypay.com";
    private const string AdminPassword = "Admin@123";
    private const long MaxResponseSizeBytes = 1_000_000; // 1MB máximo

    private async Task<string?> GetAdminToken()
    {
        var loginRequest = new LoginRequest
        {
            Email = AdminEmail,
            Password = AdminPassword
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
    public async Task LoginResponse_ShouldBeReasonableSize()
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

        var content = await response.Content.ReadAsStringAsync();
        var sizeBytes = System.Text.Encoding.UTF8.GetByteCount(content);

        // Assert
        sizeBytes.Should().BeLessThan((int)MaxResponseSizeBytes,
            $"Response deve ser menor que {MaxResponseSizeBytes} bytes, foi {sizeBytes} bytes");
    }

    [Fact]
    public async Task GetUsersResponse_ShouldBeReasonableSize()
    {
        // Arrange
        var token = await GetAdminToken();
        if (token == null)
            return;

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await client.GetAsync($"{ApiUrl}/api/admin/users");
        var content = await response.Content.ReadAsStringAsync();
        var sizeBytes = System.Text.Encoding.UTF8.GetByteCount(content);

        // Assert
        sizeBytes.Should().BeLessThan((int)MaxResponseSizeBytes,
            $"Response deve ser menor que {MaxResponseSizeBytes} bytes, foi {sizeBytes} bytes");
    }
}

