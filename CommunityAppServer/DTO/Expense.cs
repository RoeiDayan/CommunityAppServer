using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace CommunityAppServer.DTO
{
    public class Expense
    {
        public Expense() { }
        public int ExpenseId { get; set; }

        public int ComId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateOnly ExpenseDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public Models.Expense GetExpense()
        {
            Models.Expense exp = new Models.Expense();
            exp.ExpenseId = ExpenseId;
            exp.ComId = ComId;
            exp.Title = Title;
            exp.Description = Description;
            exp.Amount = Amount;
            exp.ExpenseDate = ExpenseDate;
            exp.CreatedAt = CreatedAt;
            return exp;
        }
        public Expense(Models.Expense exp)
        {
            this.ExpenseId = exp.ExpenseId;
            this.ComId = exp.ComId;
            this.Title = exp.Title;
            this.Description = exp.Description;
            this.Amount = exp.Amount;
            this.ExpenseDate = exp.ExpenseDate;
            this.CreatedAt = exp.CreatedAt;
        }
    }
}
