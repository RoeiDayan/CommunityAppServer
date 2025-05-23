using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CommunityAppServer.Models;

public partial class CommunityDBContext : DbContext
{
    public Account? GetAccount(string email)
    {
        Models.Account? acc = this.Accounts.Where(a => a.Email == email).FirstOrDefault();
        return acc;
    }
    public Account? GetAccount(int id)
    {
        Models.Account? acc = this.Accounts.Where(a => a.Id == id).FirstOrDefault();
        return acc;
    }

    public List<Member>? GetAccountMembers(int id)
    {
        List<Models.Member>? mems = this.Members.Where(m => m.UserId == id).ToList();
        return mems;
    }


}
