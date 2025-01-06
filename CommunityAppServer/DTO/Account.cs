using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class Account
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Name { get; set; }

        [StringLength(255)]
        public string? Password { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }

        public Account() 
        {
            
        }

        public Models.Account GetAccount()
        {
            Models.Account acc = new Models.Account();
            acc.Id = Id;
            acc.Email = this.Email;
            acc.Name  = this.Name;
            acc.Password = this.Password;
            acc.CreatedAt = this.CreatedAt;
            return acc;
        }
        public Account(Models.Account acc)
        {
            this.Id = acc.Id;
            this.Email = acc.Email;
            this.Name = acc.Name;
            this.Password = acc.Password;
            this.CreatedAt = acc.CreatedAt;
        }
    }
}
