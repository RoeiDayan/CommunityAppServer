﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[Table("Account")]
[Index("Email", Name = "UQ__Account__A9D105343D62E18E", IsUnique = true)]
public partial class Account
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string Password { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    [InverseProperty("User")]
    public virtual ICollection<Notice> Notices { get; set; } = new List<Notice>();

    [InverseProperty("User")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("User")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    [InverseProperty("User")]
    public virtual ICollection<RoomRequest> RoomRequests { get; set; } = new List<RoomRequest>();

    [InverseProperty("KeyHolder")]
    public virtual ICollection<TenantRoom> TenantRooms { get; set; } = new List<TenantRoom>();
}
