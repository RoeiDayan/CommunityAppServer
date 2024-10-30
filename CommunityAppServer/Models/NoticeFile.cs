using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[PrimaryKey("NoticeId", "FileName")]
public partial class NoticeFile
{
    [Key]
    public int NoticeId { get; set; }

    [Key]
    [StringLength(255)]
    public string FileName { get; set; } = null!;

    [StringLength(5)]
    public string? NoticeFileExt { get; set; }

    [ForeignKey("NoticeId")]
    [InverseProperty("NoticeFiles")]
    public virtual Notice Notice { get; set; } = null!;
}
