namespace CommunityAppServer.DTO
{
    public class MemberCommunity
    {
        public MemberCommunity() { }
        public Member Member { get; set; }  // Membership data
        public Community Community { get; set; }  // Community data
    }
}
