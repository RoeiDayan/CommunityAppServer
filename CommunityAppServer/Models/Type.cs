using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

public partial class Type
{
    [Key]
    public int TypeNum { get; set; }

    [Column("Type")]
    [StringLength(10)]
    public string? Type1 { get; set; }

    [InverseProperty("TypeNavigation")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
