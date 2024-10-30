using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

public partial class Notice
{
    [Key]
    public int NoticeId { get; set; }

    public int? UserId { get; set; }

    [StringLength(100)]
    public string? Title { get; set; }

    [Column(TypeName = "text")]
    public string? Text { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Notice")]
    public virtual ICollection<NoticeFile> NoticeFiles { get; set; } = new List<NoticeFile>();

    [ForeignKey("UserId")]
    [InverseProperty("Notices")]
    public virtual Account? User { get; set; }

    [ForeignKey("NoticeId")]
    [InverseProperty("Notices")]
    public virtual ICollection<Community> Coms { get; set; } = new List<Community>();
}
