using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class NoticeFile
    {
        [Key]
        public int NoticeId { get; set; }

        [Key]
        [StringLength(255)]
        public string FileName { get; set; } = null!;

        [StringLength(5)]
        public string? NoticeFileExt { get; set; }

        //[ForeignKey("NoticeId")]
        //[InverseProperty("NoticeFiles")]
        //public virtual Notice Notice { get; set; } = null!;

        public Models.NoticeFile GetNoticeFile()
        {
            Models.NoticeFile ntf = new Models.NoticeFile();
            ntf.NoticeId = NoticeId;
            ntf.FileName = FileName;
            ntf.NoticeFileExt = NoticeFileExt;
            return ntf;
        }
        public NoticeFile(Models.NoticeFile ntf)
        {
            this.NoticeId = ntf.NoticeId;
            this.FileName = ntf.FileName;
            this.NoticeFileExt= ntf.NoticeFileExt;
        }

    }
}
