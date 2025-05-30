﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[Table("Community")]
[Index("ComCode", Name = "UQ__Communit__5BCA59DCF5DF9015", IsUnique = true)]
public partial class Community
{
    [Key]
    public int ComId { get; set; }

    [StringLength(50)]
    public string ComName { get; set; } = null!;

    public string? ComDesc { get; set; }

    [StringLength(50)]
    public string ComCode { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string? GatePhoneNum { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Com")]
    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

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
}
