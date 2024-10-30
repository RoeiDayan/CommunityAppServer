using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[Table("Status")]
public partial class Status
{
    [Key]
    public int StatNum { get; set; }

    [Column("Status")]
    [StringLength(10)]
    public string? Status1 { get; set; }

    [InverseProperty("StatusNavigation")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
