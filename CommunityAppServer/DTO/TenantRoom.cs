using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class TenantRoom
    {
        [Key]
        public int ComId { get; set; }

        [StringLength(10)]
        public string? Status { get; set; }

        [Key]
        public int KeyHolderId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? SessionStart { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? SessionEnd { get; set; }

        //[ForeignKey("ComId")]
        //[InverseProperty("TenantRooms")]
        //public virtual Community Com { get; set; } = null!;

        //[ForeignKey("KeyHolderId")]
        //[InverseProperty("TenantRooms")]
        //public virtual Account KeyHolder { get; set; } = null!;

        public TenantRoom() { }
        public Models.TenantRoom GetTenantRoom()
        {
            Models.TenantRoom gtr = new Models.TenantRoom();
            gtr.ComId = ComId;
            gtr.Status = Status;
            gtr.KeyHolderId = KeyHolderId;
            gtr.SessionStart = SessionStart;
            gtr.SessionEnd = SessionEnd;
            return gtr;
        }
        public TenantRoom(Models.TenantRoom gtr)
        {
            this.ComId = gtr.ComId;
            this.Status = gtr.Status;
            this.KeyHolderId= gtr.KeyHolderId;
            this.SessionStart = gtr.SessionStart;
            this.SessionEnd = gtr.SessionEnd;
        }
    }
}
