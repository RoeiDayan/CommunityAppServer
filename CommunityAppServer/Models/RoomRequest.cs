using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

public partial class RoomRequest
{
    [Key]
    public int RequestId { get; set; }

    public int? UserId { get; set; }

    public int? ComId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    [Column(TypeName = "text")]
    public string? Text { get; set; }

    public bool? IsApproved { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ComId")]
    [InverseProperty("RoomRequests")]
    public virtual Community? Com { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RoomRequests")]
    public virtual Account? User { get; set; }
}
