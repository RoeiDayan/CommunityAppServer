using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[Table("Priority")]
public partial class Priority
{
    [Key]
    public int PriorityNum { get; set; }

    [Column("Priority")]
    [StringLength(20)]
    public string Priority1 { get; set; } = null!;

    [InverseProperty("PriorityNavigation")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
