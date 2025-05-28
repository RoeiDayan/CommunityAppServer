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

    // Add this method to update account information
    public void UpdateAccount(Account account)
    {
        try
        {
            this.Entry(account).State = EntityState.Modified;
            this.SaveChanges();
        }
        catch (Exception ex)
        {
            // Handle exception - you might want to log this
            throw new Exception($"Failed to update account: {ex.Message}", ex);
        }
    }
}
