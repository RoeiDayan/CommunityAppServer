using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityAppServer.Models;

namespace CommunityAppServer.DTO
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        public int UserId { get; set; }

        public int ComId { get; set; }

        [Column(TypeName = "text")]
        public string? Text { get; set; }

        public int Priority { get; set; }

        public int Status { get; set; }

        public bool? IsAnon { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        public string Title { get; set; } = null!;

        //[ForeignKey("ComId")]
        //[InverseProperty("Reports")]
        //public virtual Community? Com { get; set; }

        //[ForeignKey("Priority")]
        //[InverseProperty("Reports")]
        //public virtual Priority? PriorityNavigation { get; set; }

        //[InverseProperty("Report")]
        //public virtual ICollection<ReportFile> ReportFiles { get; set; } = new List<ReportFile>();

        //[ForeignKey("Status")]
        //[InverseProperty("Reports")]
        //public virtual Status? StatusNavigation { get; set; }

        //[ForeignKey("Type")]
        //[InverseProperty("Reports")]
        //public virtual Type? TypeNavigation { get; set; }

        //[ForeignKey("UserId")]
        //[InverseProperty("Reports")]
        //public virtual Account? User { get; set; }

        public Models.Report GetReport()
        {
            Models.Report rep = new Models.Report();
            rep.ReportId = ReportId;
            rep.UserId = UserId;
            rep.ComId = ComId;
            rep.Text = Text;
            rep.Priority = Priority;
            rep.Status = Status;
            rep.IsAnon = IsAnon;
            rep.CreatedAt = CreatedAt;
            return rep;
        }
        public Report(Models.Report rep)
        {
            this.ReportId = rep.ReportId;
            this.UserId = rep.UserId;
            this.ComId = rep.ComId;
            this.Text = rep.Text;
            this.Priority = rep.Priority;
            this.Status = rep.Status;
            this.IsAnon = rep.IsAnon;
            this.CreatedAt = rep.CreatedAt;
        }
    }
}
