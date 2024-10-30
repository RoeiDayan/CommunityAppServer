using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class Notice
    {
        [Key]
        public int NoticeId { get; set; }

        public int? UserId { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [Column(TypeName = "text")]
        public string? Text { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? StartTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? EndTime { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        //[InverseProperty("Notice")]
        //public virtual ICollection<NoticeFile> NoticeFiles { get; set; } = new List<NoticeFile>();

        //[ForeignKey("UserId")]
        //[InverseProperty("Notices")]
        //public virtual Account? User { get; set; }

        //[ForeignKey("NoticeId")]
        //[InverseProperty("Notices")]
        //public virtual ICollection<Community> Coms { get; set; } = new List<Community>();

        public Models.Notice GetNotice()
        {
            Models.Notice ntc = new Models.Notice();
            ntc.NoticeId = NoticeId;
            ntc.UserId = UserId;
            ntc.Title = Title;
            ntc.Text = Text;
            ntc.StartTime = StartTime;
            ntc.EndTime = EndTime;
            ntc.CreatedAt = CreatedAt;
            return ntc;
        }
        public Notice(Models.Notice ntc)
        {
            this.NoticeId = ntc.NoticeId;
            this.UserId = ntc.UserId;
            this.Title = ntc.Title;
            this.Text = ntc.Text;
            this.StartTime = ntc.StartTime;
            this.EndTime = ntc.EndTime;
            this.CreatedAt = ntc.CreatedAt;
        }
    }
}
