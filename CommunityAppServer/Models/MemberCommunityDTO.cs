using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models
{
    public class MemberCommunityDTO
    {
        public Member Member { get; set; }  // Membership data
        public Community Community { get; set; }  // Community data
    }

}
