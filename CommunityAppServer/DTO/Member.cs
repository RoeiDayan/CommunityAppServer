using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class Member
    {
        public Member() { }
        public int ComId { get; set; }

        [Key]
        public int UserId { get; set; }

        [StringLength(20)]
        public string? Role { get; set; }

        public int? Balance { get; set; }

        public int? UnitNum { get; set; }

        public bool? IsLiable { get; set; }

        public bool? IsResident { get; set; }

        public bool? IsManager { get; set; }

        public bool? IsProvider { get; set; }

        public bool? IsApproved { get; set; }

        //[ForeignKey("ComId")]
        //[InverseProperty("Members")]
        //public virtual Community Com { get; set; } = null!;

        //[ForeignKey("UserId")]
        //[InverseProperty("Members")]
        //public virtual Account User { get; set; } = null!;

        public Models.Member GetMember()
        {
            Models.Member mem = new Models.Member();
            mem.ComId = ComId;
            mem.UserId = UserId;
            mem.Role = Role;
            mem.Balance = Balance;
            mem.UnitNum = UnitNum;
            mem.IsLiable = IsLiable;
            mem.IsResident = IsResident;
            mem.IsManager = IsManager;
            mem.IsProvider = IsProvider;
            mem.IsApproved = IsApproved;
            return mem;
        }
        public Member(Models.Member mem)
        {
            this.ComId = mem.ComId;
            this.UserId = mem.UserId;
            this.Role = mem.Role;
            this.Balance = mem.Balance;
            this.UnitNum = mem.UnitNum;
            this.IsLiable = mem.IsLiable;
            this.IsResident= mem.IsResident;
            this.IsManager = mem.IsManager;
            this.IsProvider = mem.IsProvider;
            this.IsApproved= mem.IsApproved;
        }
    }
}
