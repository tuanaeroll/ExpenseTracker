namespace ExpenseManagement.Business.Models
{
    public class BankPaymentResponseDto
    {
        public bool Success { get; set; }
        public string? TransactionId { get; set; }
        public string Message { get; set; }
    }
}
