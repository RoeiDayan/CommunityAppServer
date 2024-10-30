using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;

namespace CommunityAppServer.DTO
{
    public class ReportFile
    {
        [Key]
        public int ReportId { get; set; }

        [Key]
        [StringLength(255)]
        public string FileName { get; set; } = null!;

        [StringLength(5)]
        public string? RepFileExt { get; set; }

        //[ForeignKey("ReportId")]
        //[InverseProperty("ReportFiles")]
        //public virtual Report Report { get; set; } = null!;

        public Models.ReportFile GetReportFile()
        {
            Models.ReportFile repf = new Models.ReportFile();
            repf.ReportId = ReportId;
            repf.FileName = FileName;
            repf.RepFileExt = RepFileExt;
            return repf;
        }
        public ReportFile(Models.ReportFile repf)
        {
            this.ReportId = repf.ReportId;
            this.FileName = repf.FileName;
            this.RepFileExt = repf.RepFileExt;
        }
    }
}
