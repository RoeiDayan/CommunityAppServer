using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class Community
    {
        public Community() { }
        public int ComId { get; set; }

        public string? ComName { get; set; }

        public string? ComDesc { get; set; }

        public string? ComCode { get; set; }


        public string? GatePhoneNum { get; set; }

        public DateTime? CreatedAt { get; set; }

        

        public Models.Community GetCommunity()
        {
            Models.Community com = new Models.Community();
            com.ComId = ComId;
            com.ComName = ComName;
            com.ComDesc = ComDesc;
            com.ComCode = ComCode;
            com.CreatedAt = CreatedAt;
            com.GatePhoneNum = GatePhoneNum;
            return com;
        }
        public Community(Models.Community com)
        {
            this.ComId = com.ComId;
            this.ComName = com.ComName;
            this.ComDesc = com.ComDesc;
            this.ComCode = com.ComCode;
            this.CreatedAt = com.CreatedAt;
            this.GatePhoneNum= com.GatePhoneNum;
        }
    }
}
