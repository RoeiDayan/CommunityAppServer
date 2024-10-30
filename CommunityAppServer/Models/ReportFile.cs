using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[PrimaryKey("ReportId", "FileName")]
public partial class ReportFile
{
    [Key]
    public int ReportId { get; set; }

    [Key]
    [StringLength(255)]
    public string FileName { get; set; } = null!;

    [StringLength(5)]
    public string? RepFileExt { get; set; }

    [ForeignKey("ReportId")]
    [InverseProperty("ReportFiles")]
    public virtual Report Report { get; set; } = null!;
}
