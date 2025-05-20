using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class RoomRequest
    {
        public RoomRequest() { }
        public int RequestId { get; set; }

        public int UserId { get; set; }

        public int ComId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime EndTime { get; set; }

        [Column(TypeName = "text")]
        public string? Text { get; set; }

        public bool? IsApproved { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        //[ForeignKey("ComId")]
        //[InverseProperty("RoomRequests")]
        //public virtual Community? Com { get; set; }

        //[ForeignKey("UserId")]
        //[InverseProperty("RoomRequests")]
        //public virtual Account? User { get; set; }

        public Models.RoomRequest GetRoomRequest()
        {
            Models.RoomRequest room = new Models.RoomRequest();
            room.RequestId = RequestId;
            room.UserId = UserId;
            room.ComId = ComId;
            room.StartTime = StartTime;
            room.EndTime = EndTime;
            room.Text = Text;
            room.IsApproved = IsApproved;
            room.CreatedAt = CreatedAt;
            return room;
        }
        public RoomRequest(Models.RoomRequest role)
        {
            this.RequestId = role.RequestId;
            this.UserId = role.UserId;
            this.ComId = role.ComId;
            this.StartTime = role.StartTime;
            this.EndTime = role.EndTime;
            this.Text = role.Text;
            this.IsApproved = role.IsApproved;
            this.CreatedAt = role.CreatedAt;
        }
    }
}
