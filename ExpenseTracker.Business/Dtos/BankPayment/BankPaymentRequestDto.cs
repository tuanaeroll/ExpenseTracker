namespace ExpenseManagement.Business.Models
{
    public class BankPaymentRequestDto
    {
        public string Iban { get; set; }
        public decimal Amount { get; set; }
        public string FullName { get; set; }
    }
}
