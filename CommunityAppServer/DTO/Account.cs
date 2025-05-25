using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommunityAppServer.DTO
{
    public class Account
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedAt { get; set; }
        public string? PhoneNumber { get; set; }

        public string? ProfilePhotoFileName { get; set; }

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
            acc.PhoneNumber = this.PhoneNumber;
            acc.ProfilePhotoFileName = this.ProfilePhotoFileName;
            return acc;
        }
        public Account(Models.Account acc)
        {
            this.Id = acc.Id;
            this.Email = acc.Email;
            this.Name = acc.Name;
            this.Password = acc.Password;
            this.CreatedAt = acc.CreatedAt;
            this.PhoneNumber = acc.PhoneNumber;
            this.ProfilePhotoFileName = acc.ProfilePhotoFileName;
        }
    }
}
