using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[Table("Community")]
public partial class Community
{
    [Key]
    public int ComId { get; set; }

    [StringLength(15)]
    public string? ComName { get; set; }

    [Column(TypeName = "text")]
    public string? Body { get; set; }

    [StringLength(20)]
    public string? ComCode { get; set; }

    [StringLength(50)]
    public string? Picture { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? GatePhoneNum { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Com")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    [InverseProperty("Com")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("Com")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    [InverseProperty("Com")]
    public virtual ICollection<RoomRequest> RoomRequests { get; set; } = new List<RoomRequest>();

    [InverseProperty("Com")]
    public virtual ICollection<TenantRoom> TenantRooms { get; set; } = new List<TenantRoom>();

    [ForeignKey("ComId")]
    [InverseProperty("Coms")]
    public virtual ICollection<Notice> Notices { get; set; } = new List<Notice>();
}
