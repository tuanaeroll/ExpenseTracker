using System.Net.Http;
using System.Text;
using System.Text.Json;
using ExpenseManagement.Business.Interfaces;
using ExpenseManagement.Business.Models;
using ExpenseTracker.Business.Logging.Interfaces;

namespace ExpenseManagement.Business.Services
{
    public class BankPaymentService : IBankPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggingService<BankPaymentService> _logger;

        public BankPaymentService(HttpClient httpClient, ILoggingService<BankPaymentService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<BankPaymentResponseDto> MakePaymentAsync(BankPaymentRequestDto request)
        {
            _logger.LogInfo("Banka ödeme isteği hazırlanıyor: FullName={FullName}, Iban={Iban}, Amount={Amount}",
                request.FullName, request.Iban, request.Amount);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("api/expensetrackerbank/payment", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInfo("Banka ödeme yanıtı alındı: StatusCode={StatusCode}, Response={ResponseContent}",
                    response.StatusCode, responseContent);

                var result = JsonSerializer.Deserialize<BankPaymentResponseDto>(
                    responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

                if (!result.Success)
                {
                    _logger.LogWarning("Banka ödeme başarısız: Message={Message}", result.Message);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Banka ödeme isteği sırasında hata oluştu.");
                throw;
            }
        }
    }
}
