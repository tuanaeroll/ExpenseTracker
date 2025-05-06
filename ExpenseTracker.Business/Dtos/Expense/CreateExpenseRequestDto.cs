using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Business.Dtos.Expense
{
    public class CreateExpenseRequestDto
    {
        public string Title { get; set; }
        public decimal Amount { get; set; } 
        public int CategoryId { get; set; } 
        public int PaymentMethodId { get; set; }
        public string? Description { get; set; }
        public IFormFile? Receipt { get; set; }
    }
}