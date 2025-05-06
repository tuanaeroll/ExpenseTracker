using System.Text.Json.Serialization;

namespace ExpenseTracker.Business.Dtos.Expense
{
    public class ExpenseResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string CategoryName { get; set; }
        public string PaymentMethodName { get; set; }
        public string? Description { get; set; }
        public string? UserFullName { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime? PaidAt { get; set; }
        public string CreatedAtFormatted => CreatedAt.ToString("dd.MM.yyyy HH:mm");

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PaidAtFormatted => PaidAt?.ToString("dd.MM.yyyy HH:mm");
    }
}
