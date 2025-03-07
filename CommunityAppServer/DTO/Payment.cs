using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityAppServer.Models;

namespace CommunityAppServer.DTO
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public int ComId { get; set; }

        public int UserId { get; set; }

        public int Amount { get; set; }

        public bool? MarkedPayed { get; set; }

        public bool? WasPayed { get; set; }

        public DateOnly PayFrom { get; set; }

        public DateOnly PayUntil { get; set; }

        //[ForeignKey("ComId")]
        //[InverseProperty("Payments")]
        //public virtual Community? Com { get; set; }

        //[ForeignKey("UserId")]
        //[InverseProperty("Payments")]
        //public virtual Account? User { get; set; }
        public Models.Payment GetPayment()
        {
            Models.Payment pay = new Models.Payment();
            pay.PaymentId = PaymentId;
            pay.ComId = ComId;
            pay.UserId = UserId;
            pay.Amount = Amount;
            pay.MarkedPayed = MarkedPayed;
            pay.WasPayed = WasPayed;
            pay.PayFrom = PayFrom;
            pay.PayUntil = PayUntil;
            return pay;
        }
        public Payment(Models.Payment pay)
        {
            this.PaymentId = pay.PaymentId;
            this.ComId = pay.ComId;
            this.UserId = pay.UserId;
            this.Amount = pay.Amount;
            this.MarkedPayed= pay.MarkedPayed;
            this.WasPayed= pay.WasPayed;
            this.PayFrom = pay.PayFrom;
            this.PayUntil = pay.PayUntil;
        }
    }
}
