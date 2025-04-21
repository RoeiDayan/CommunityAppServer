using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[PrimaryKey("ComId", "KeyHolderId")]
[Table("TenantRoom")]
public partial class TenantRoom
{
    [Key]
    public int ComId { get; set; }

    [StringLength(20)]
    public string? Status { get; set; }

    [Key]
    public int KeyHolderId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SessionStart { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SessionEnd { get; set; }

    [ForeignKey("ComId")]
    [InverseProperty("TenantRooms")]
    public virtual Community Com { get; set; } = null!;

    [ForeignKey("KeyHolderId")]
    [InverseProperty("TenantRooms")]
    public virtual Account KeyHolder { get; set; } = null!;
}
