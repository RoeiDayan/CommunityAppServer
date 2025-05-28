using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

public partial class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int ComId { get; set; }

    public int UserId { get; set; }

    public int Amount { get; set; }

    [StringLength(200)]
    public string? Details { get; set; }

    public bool? WasPayed { get; set; }

    public DateOnly? PayFrom { get; set; }

    public DateOnly? PayUntil { get; set; }

    [ForeignKey("ComId")]
    [InverseProperty("Payments")]
    public virtual Community Com { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Payments")]
    public virtual Account User { get; set; } = null!;
}
