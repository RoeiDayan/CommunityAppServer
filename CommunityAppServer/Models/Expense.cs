using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

public partial class Expense
{
    [Key]
    public int ExpenseId { get; set; }

    public int ComId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    public DateOnly ExpenseDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("ComId")]
    [InverseProperty("Expenses")]
    public virtual Community Com { get; set; } = null!;
}
