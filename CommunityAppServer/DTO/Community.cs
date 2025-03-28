using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class Community
    {
        public Community() { }
        public int ComId { get; set; }

        [StringLength(15)]
        public string? ComName { get; set; }

        [Column(TypeName = "text")]
        public string? ComDesc { get; set; }

        [StringLength(20)]
        public string? ComCode { get; set; }

        [StringLength(50)]
        public string? Picture { get; set; }

        [StringLength(15)]
        [Unicode(false)]
        public string? GatePhoneNum { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        //[InverseProperty("Com")]
        //public virtual ICollection<Member> Members { get; set; } = new List<Member>();

        //[InverseProperty("Com")]
        //public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        //[InverseProperty("Com")]
        //public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

        //[InverseProperty("Com")]
        //public virtual ICollection<RoomRequest> RoomRequests { get; set; } = new List<RoomRequest>();

        //[InverseProperty("Com")]
        //public virtual ICollection<TenantRoom> TenantRooms { get; set; } = new List<TenantRoom>();

        //[ForeignKey("ComId")]
        //[InverseProperty("Coms")]
        //public virtual ICollection<Notice> Notices { get; set; } = new List<Notice>();


        public Models.Community GetCommunity()
        {
            Models.Community com = new Models.Community();
            com.ComId = ComId;
            com.ComName = ComName;
            com.ComDesc = ComDesc;
            com.ComCode = ComCode;
            com.Picture = Picture;
            com.CreatedAt = CreatedAt;
            com.GatePhoneNum = GatePhoneNum;
            return com;
        }
        public Community(Models.Community com)
        {
            this.ComId = com.ComId;
            this.ComName = com.ComName;
            this.ComDesc = com.ComDesc;
            this.ComCode = com.ComCode;
            this.Picture = com.Picture;
            this.CreatedAt = com.CreatedAt;
            this.GatePhoneNum= com.GatePhoneNum;
        }
    }
}
