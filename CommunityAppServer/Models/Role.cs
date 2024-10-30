using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

[Table("Role")]
public partial class Role
{
    [Key]
    public int RoleNum { get; set; }

    [StringLength(10)]
    public string? RoleName { get; set; }
}
