using CommunityAppServer.Models;

namespace CommunityAppServer.DTO
{
    public class PaymentMemberAccount
    {
        public PaymentMemberAccount() { }
        public Payment Payment { get; set; }
        public Member Member { get; set; }
        public Account Account { get; set; }
    }
}
