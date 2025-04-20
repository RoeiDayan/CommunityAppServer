using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[Table("Community")]
[Index("ComCode", Name = "UQ__Communit__5BCA59DC1E76CA27", IsUnique = true)]
public partial class Community
{
    [Key]
    public int ComId { get; set; }

    [StringLength(50)]
    public string ComName { get; set; } = null!;

    public string? ComDesc { get; set; }

    [StringLength(50)]
    public string ComCode { get; set; } = null!;

    [StringLength(255)]
    public string? Picture { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? GatePhoneNum { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Com")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    [InverseProperty("Com")]
    public virtual ICollection<Notice> Notices { get; set; } = new List<Notice>();

    [InverseProperty("Com")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("Com")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    [InverseProperty("Com")]
    public virtual ICollection<RoomRequest> RoomRequests { get; set; } = new List<RoomRequest>();

    [InverseProperty("Com")]
    public virtual ICollection<TenantRoom> TenantRooms { get; set; } = new List<TenantRoom>();
}
