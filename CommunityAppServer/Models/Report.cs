using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[Table("Report")]
public partial class Report
{
    [Key]
    public int ReportId { get; set; }

    public int? UserId { get; set; }

    public int? ComId { get; set; }

    [Column(TypeName = "text")]
    public string? Text { get; set; }

    public int? Priority { get; set; }

    public int? Status { get; set; }

    public bool? IsAnon { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    [ForeignKey("ComId")]
    [InverseProperty("ReportsNavigation")]
    public virtual Community? Com { get; set; }

    [ForeignKey("Priority")]
    [InverseProperty("Reports")]
    public virtual Priority? PriorityNavigation { get; set; }

    [InverseProperty("Report")]
    public virtual ICollection<ReportFile> ReportFiles { get; set; } = new List<ReportFile>();

    [ForeignKey("Status")]
    [InverseProperty("Reports")]
    public virtual Status? StatusNavigation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Reports")]
    public virtual Account? User { get; set; }

    [ForeignKey("ReportId")]
    [InverseProperty("Reports")]
    public virtual ICollection<Community> Coms { get; set; } = new List<Community>();
}
