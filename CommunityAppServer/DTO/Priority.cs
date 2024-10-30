using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityAppServer.Models;

namespace CommunityAppServer.DTO
{
    public class Priority
    {
        [Key]
        public int PriorityNum { get; set; }

        [Column("Priority")]
        [StringLength(10)]
        public string? Priority1 { get; set; }

        //[InverseProperty("PriorityNavigation")]
        //public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

        public Models.Priority GetPriority()
        {
            Models.Priority pri = new Models.Priority();
            pri.PriorityNum = PriorityNum;
            pri.Priority1 = Priority1;
            return pri;
        }
        public Priority(Models.Priority pri)
        {
            this.PriorityNum = pri.PriorityNum;
            this.Priority1 = pri.Priority1;
        }
    }
}
