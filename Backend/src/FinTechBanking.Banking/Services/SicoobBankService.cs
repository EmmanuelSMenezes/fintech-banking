using System.Text;
using System.Text.Json;
using FinTechBanking.Core.Interfaces;

namespace FinTechBanking.Banking.Services;

public class SicoobBankService : ISicoobBankService
{
    private readonly string _clientId;
    private readonly string _accessToken;
    private readonly string _apiUrl;
    private readonly HttpClient _httpClient;

    public SicoobBankService(string clientId, string accessToken, string apiUrl = "https://api.sicoob.com.br")
    {
        _clientId = clientId;
        _accessToken = accessToken;
        _apiUrl = apiUrl;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<string> GeneratePixQrCodeAsync(decimal amount, string recipientKey, string description)
    {
        try
        {
            Console.WriteLine($"[Sicoob] Gerando QR Code PIX: {amount} para {recipientKey}");

            // Endpoint: POST /pix/qrcode
            var request = new
            {
                amount = (int)(amount * 100), // Converter para centavos
                recipientKey = recipientKey,
                description = description,
                expirationTime = 3600 // 1 hora
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/pix/qrcode", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(responseContent);
                var qrCode = jsonDoc.RootElement.GetProperty("qrCode").GetString();

                Console.WriteLine($"[Sicoob] QR Code gerado com sucesso: {qrCode}");
                return qrCode ?? throw new Exception("QR Code não retornado");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[Sicoob] Erro ao gerar QR Code: {error}");
                throw new Exception($"Erro Sicoob: {response.StatusCode} - {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Sicoob] Exceção ao gerar QR Code: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> ProcessWithdrawalAsync(decimal amount, string accountNumber)
    {
        try
        {
            Console.WriteLine($"[Sicoob] Processando saque: {amount} da conta {accountNumber}");

            // Endpoint: POST /transfers/withdrawal
            var request = new
            {
                amount = (int)(amount * 100), // Converter para centavos
                accountNumber = accountNumber,
                description = "Saque via FinTech"
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}/transfers/withdrawal", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[Sicoob] Saque processado com sucesso");
                return true;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[Sicoob] Erro ao processar saque: {error}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Sicoob] Exceção ao processar saque: {ex.Message}");
            return false;
        }
    }

    public async Task<decimal> GetBalanceAsync(string accountNumber)
    {
        try
        {
            Console.WriteLine($"[Sicoob] Obtendo saldo da conta {accountNumber}");

            // Endpoint: GET /accounts/{accountNumber}/balance
            var response = await _httpClient.GetAsync($"{_apiUrl}/accounts/{accountNumber}/balance");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(responseContent);
                var balance = jsonDoc.RootElement.GetProperty("balance").GetDecimal();

                Console.WriteLine($"[Sicoob] Saldo obtido: {balance}");
                return balance / 100; // Converter de centavos
            }
            else
            {
                Console.WriteLine($"[Sicoob] Erro ao obter saldo: {response.StatusCode}");
                return 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Sicoob] Exceção ao obter saldo: {ex.Message}");
            return 0;
        }
    }
}

