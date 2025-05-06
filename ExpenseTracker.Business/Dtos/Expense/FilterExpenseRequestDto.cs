using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Data.Enums;

namespace ExpenseTracker.Business.Dtos.Expense
{
    public class FilterExpenseRequestDto
    {
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaidAt { get; set; }

        public int? CategoryId { get; set; }
        public int? PaymentMethodId { get; set; }
        public ExpenseStatus? Status { get; set; }
    }
}

