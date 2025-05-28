using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityAppServer.Models;

namespace CommunityAppServer.DTO
{
    public class Report
    {
        public Report() { }
        public int ReportId { get; set; }

        public int UserId { get; set; }

        public int ComId { get; set; }

        public string Title { get; set; } = null!;

        public string? ReportDesc { get; set; }

        public int? Priority { get; set; }

        public int? Status { get; set; }


        public DateTime? CreatedAt { get; set; }

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
            rep.ReportDesc = ReportDesc;
            rep.CreatedAt = CreatedAt;
            rep.Title = Title;
            return rep;
        }
        public Report(Models.Report rep)
        {
            this.ReportId = rep.ReportId;
            this.UserId = rep.UserId;
            this.ComId = rep.ComId;
            this.ReportDesc = rep.ReportDesc;
            this.CreatedAt = rep.CreatedAt;
            this.Title = rep.Title;
        }
    }
}
