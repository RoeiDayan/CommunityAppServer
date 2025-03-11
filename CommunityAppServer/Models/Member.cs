using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[PrimaryKey("ComId", "UserId")]
public partial class Member
{
    [Key]
    public int ComId { get; set; }

    [Key]
    public int UserId { get; set; }

    [StringLength(50)]
    public string? Role { get; set; }

    public int? Balance { get; set; }

    public int? UnitNum { get; set; }

    public bool? IsLiable { get; set; }

    public bool? IsResident { get; set; }

    public bool? IsManager { get; set; }

    public bool? IsProvider { get; set; }

    public bool? IsApproved { get; set; }

    [ForeignKey("ComId")]
    [InverseProperty("Members")]
    public virtual Community Com { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Members")]
    public virtual Account User { get; set; } = null!;
}
