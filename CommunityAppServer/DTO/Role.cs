using CommunityAppServer.Models;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class Role
    {
        [Key]
        public int RoleNum { get; set; }

        [StringLength(10)]
        public string? RoleName { get; set; }

        public Models.Role GetRole()
        {
            Models.Role role = new Models.Role();
            role.RoleNum = RoleNum;
            role.RoleName = RoleName;
            return role;
        }
        public Role(Models.Role role)
        {
            this.RoleNum = role.RoleNum;
            this.RoleName = role.RoleName;
        }
    }
}
