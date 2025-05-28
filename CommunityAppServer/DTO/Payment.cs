using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CommunityAppServer.Models;

namespace CommunityAppServer.DTO
{
    public class Payment
    {
        public Payment() { }
        public int PaymentId { get; set; }

        public int ComId { get; set; }

        public int UserId { get; set; }

        public int Amount { get; set; }

        public string? Details { get; set; }


        public bool? WasPayed { get; set; }

        public DateOnly? PayFrom { get; set; }

        public DateOnly? PayUntil { get; set; }

        public Models.Payment GetPayment()
        {
            Models.Payment pay = new Models.Payment();
            pay.PaymentId = PaymentId;
            pay.ComId = ComId;
            pay.UserId = UserId;
            pay.Amount = Amount;
            pay.Details = Details;
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
            this.Details = pay.Details;
            this.WasPayed= pay.WasPayed;
            this.PayFrom = pay.PayFrom;
            this.PayUntil = pay.PayUntil;
        }
    }
}
