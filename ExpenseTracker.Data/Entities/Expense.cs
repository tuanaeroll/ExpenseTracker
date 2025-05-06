using ExpenseTracker.Data.Enums;

namespace ExpenseTracker.Data.Entities
{
    public class Expense : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;
        public string? ResponseNote { get; set; }

        public int? ApprovedById { get; set; }
        public User? ApprovedBy { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public string? Location { get; set; }
        public string? ReceiptFilePath { get; set; }

        public DateTime? PaidAt { get; set; }
        public string? ReferenceCode { get; set; }
    }
}
