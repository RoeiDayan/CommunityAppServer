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
}
