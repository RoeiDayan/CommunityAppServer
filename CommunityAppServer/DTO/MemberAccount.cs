namespace CommunityAppServer.DTO
{
    public class MemberAccount
    {
        public Member Member { get; set; }  // Membership data
        public Account Account { get; set; }  // Community data
        public MemberAccount() { }
    }
}
