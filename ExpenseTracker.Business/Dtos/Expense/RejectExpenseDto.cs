using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Business.Dtos.Expense
{
    public class RejectExpenseDto
    {
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "Red sebebi zorunludur.")]
        public string ResponseNote { get; set; }
    }
}
