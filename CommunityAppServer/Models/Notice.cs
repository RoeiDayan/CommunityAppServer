using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[Index("ComId", Name = "IX_Notices_ComId")]
[Index("EndTime", Name = "IX_Notices_EndTime")]
[Index("StartTime", Name = "IX_Notices_StartTime")]
[Index("UserId", Name = "IX_Notices_UserId")]
public partial class Notice
{
    [Key]
    public int NoticeId { get; set; }

    public int UserId { get; set; }

    public int ComId { get; set; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    public string? NoticeDesc { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ComId")]
    [InverseProperty("Notices")]
    public virtual Community Com { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Notices")]
    public virtual Account User { get; set; } = null!;
}
