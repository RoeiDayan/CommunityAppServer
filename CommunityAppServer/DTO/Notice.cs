using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class Notice
    {
        public int NoticeId { get; set; }

        public int UserId { get; set; }
        public int ComId { get; set; }

        public string Title { get; set; }

        public string? NoticeDesc { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Notice() { }

        public Models.Notice GetNotice()
        {
            Models.Notice ntc = new Models.Notice();
            ntc.NoticeId = NoticeId;
            ntc.ComId = ComId;
            ntc.UserId = UserId;
            ntc.Title = Title;
            ntc.NoticeDesc = NoticeDesc;
            ntc.StartTime = StartTime;
            ntc.EndTime = EndTime;
            ntc.CreatedAt = CreatedAt;
            return ntc;
        }
        public Notice(Models.Notice ntc)
        {
            this.NoticeId = ntc.NoticeId;
            this.ComId = ntc.ComId;
            this.UserId = ntc.UserId;
            this.Title = ntc.Title;
            this.NoticeDesc = ntc.NoticeDesc;
            this.StartTime = ntc.StartTime;
            this.EndTime = ntc.EndTime;
            this.CreatedAt = ntc.CreatedAt;
        }
    }
}
