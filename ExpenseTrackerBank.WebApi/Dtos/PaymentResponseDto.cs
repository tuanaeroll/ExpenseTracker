namespace ExpenseTrackerBank.WebApi.Dtos
{
    public class PaymentResponseDto
    {
        public bool Success { get; set; }
        public string? TransactionId { get; set; }
        public string Message { get; set; }
    }
}
