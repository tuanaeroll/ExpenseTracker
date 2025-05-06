namespace ExpenseTrackerBank.WebApi.Dtos
{
    public class PaymentRequestDto
    {
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public string FullName { get; set; }
    }
}
