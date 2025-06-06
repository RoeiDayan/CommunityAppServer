﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[Table("Report")]
[Index("CreatedAt", Name = "IX_Report_CreatedAt")]
[Index("UserId", "ComId", Name = "IX_Report_UserId_ComId")]
public partial class Report
{
    [Key]
    public int ReportId { get; set; }

    public int UserId { get; set; }

    public int ComId { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    public string? ReportDesc { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ComId")]
    [InverseProperty("Reports")]
    public virtual Community Com { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Reports")]
    public virtual Account User { get; set; } = null!;
}
