﻿using Microsoft.AspNetCore.Mvc;
using CommunityAppServer.Models;
using Microsoft.EntityFrameworkCore;
using CommunityAppServer.DTO;

namespace CommunityAppServer.Controllers;

[Route("api")]
[ApiController]
public class CommunityAppServerAPIController : ControllerBase
{
    //a variable to hold a reference to the db context!
    private CommunityDBContext context;
    //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
    private IWebHostEnvironment webHostEnvironment;
    //Use dependency injection to get the db context and web host into the constructor
    public CommunityAppServerAPIController(CommunityDBContext context, IWebHostEnvironment env)
    {
        this.context = context;
        this.webHostEnvironment = env;
    }

    [HttpGet]
    [Route("TestServer")]
    public ActionResult<string> TestServer()
    {
        return Ok("Server Responded Successfully");
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] DTO.LoginInfo loginDto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Get model user class from DB with matching email. 
            Models.Account? modelsAccount = context.GetAccount(loginDto.Email);

            //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
            if (modelsAccount == null || modelsAccount.Password != loginDto.Password)
            {
                return Unauthorized();
            }

            //Login suceed! now mark login in session memory!
            HttpContext.Session.SetString("loggedInAccount", loginDto.Email);

            DTO.Account dtoAccount = new DTO.Account(modelsAccount);
            //dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
            return Ok(dtoAccount);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] DTO.Account accountDto)
    {
        try
        {
            HttpContext.Session.Clear(); //Logout any previous login attempt

            //Create model user class
            Models.Account modelsAccount = accountDto.GetAccount();

            context.Accounts.Add(modelsAccount);
            context.SaveChanges();

            //User was added!
            DTO.Account dtoAccount = new DTO.Account(modelsAccount);
            //dtoAccount.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
            return Ok(dtoAccount);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }



    [HttpGet("GetUserCommunities")]
    public IActionResult GetUserCommunities(int id)
    {
        try
        {
            string? userEmail = HttpContext.Session.GetString("loggedInAccount");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            Models.Account? account = context.GetAccount(userEmail);
            context.ChangeTracker.Clear();

            if (account == null || (account.Id != id))
            {
                return Unauthorized("User ID does not match");
            }

            List<DTO.MemberCommunity> memberCommunities = context.Members
                .Where(m => m.UserId == id)
                .Select(m => new DTO.MemberCommunity
                {
                    Member = new DTO.Member(m),
                    Community = new DTO.Community(context.Communities.FirstOrDefault(c => c.ComId == m.ComId))
                })
                .ToList();

            return Ok(memberCommunities);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //[HttpPost("SignInToCommunity")]
    //public IActionResult SignInToCommunity([FromBody] int comId)
    //{
    //    try
    //    {
    //        // Check if the user is logged in
    //        string? userEmail = HttpContext.Session.GetString("loggedInAccount");
    //        if (string.IsNullOrEmpty(userEmail))
    //        {
    //            return Unauthorized("User is not logged in");
    //        }

    //        // Get the account of the logged-in user
    //        Models.Account? account = context.GetAccount(userEmail);
    //        if (account == null)
    //        {
    //            return Unauthorized("Invalid user session");
    //        }

    //        // Check if the user is a member of the community
    //        bool isMember = context.Members.Any(m => m.UserId == account.Id && m.ComId == comId);
    //        if (!isMember)
    //        {
    //            return Unauthorized("User is not a member of this community");
    //        }

    //        // Store the selected community in the session
    //        HttpContext.Session.SetInt32("selectedCommunityId", comId);

    //        return Ok("Sign-in to community successful");
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}

    [HttpGet("GetCommunityNotices")]
    public IActionResult GetCommunityNotices(int ComId)
    {
        try
        {
            List<DTO.Notice> comNotices = context.Notices
            .Where(n => n.Coms.Any(c => c.ComId == ComId))
            .Select(n => new DTO.Notice(n))
            .ToList();


            return Ok(comNotices);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetCommunityReports")]
    public IActionResult GetCommunityReports(int ComId)
    {
        try
        {

            List<DTO.Report> comReports = context.Reports
            .Where(r => r.ComId == ComId)
            .Select(r=>new DTO.Report(r))
            .ToList();

            return Ok(comReports);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("CreateReport")]
    public IActionResult CreateReport([FromBody] DTO.Report rep)
    {
        try
        {
            Models.Report modelsReport = rep.GetReport();
            if (modelsReport == null)
            {
                return BadRequest("Invalid report data.");
            }

            context.Reports.Add(modelsReport);

            int changes = context.SaveChanges();

            if (changes > 0)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }
        catch (Exception ex)
        {

            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpPost("GetCommunityId")]
    public IActionResult GetCommunityId([FromBody] string s)
    {
        try
        {
            if (s == null)
            {
                return BadRequest("Invalid community code.");
            }
            bool IsCodeValid = this.context.Communities.Any(c=>c.ComCode == s);
            if(IsCodeValid)
            {
                int ID = this.context.Communities.Where(c => c.ComCode == s).Select(c=>c.ComId).FirstOrDefault();
                return Ok(ID);
            }
            else
            {
                return Ok(-1);
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");  
        }
    }
    [HttpPost("JoinCommunity")]
    public IActionResult JoinCommunity([FromBody] DTO.Member member)
    {
        try
        {
            if (member == null)
            {
                return BadRequest("Invalid member data.");
            }

            bool isMember = context.Members.Any(m => m.UserId == member.UserId && m.ComId == member.ComId);
            if (isMember)
            {
                return BadRequest("User is already a member of this community");
            }

            context.Members.Add(member.GetMember());
            int changes = context.SaveChanges();

            if (changes > 0)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
            
        }
        catch (Exception ex)
        {

            return BadRequest($"Error: {ex.Message}");
        }
    }

    [HttpPost("CreateCommunity")]
    public IActionResult CreateCommunity([FromBody] DTO.MemberCommunity MemCom)
    {
        DTO.Community community = MemCom.Community;
        DTO.Member member = MemCom.Member;
        try
        {
            string s = community.ComCode;
            if (s == null)
            {
                return BadRequest("Invalid community code.");
            }
            bool doesCodeExist = this.context.Communities.Any(c => c.ComCode == s);
            if (doesCodeExist)
            {
                return BadRequest("Community Code Exists");
            }
            else
            {
                Models.Community modelCom = community.GetCommunity();
                context.Communities.Add(modelCom);
                context.SaveChanges();
                IActionResult result = GetCommunityId(s);
                if (result is OkObjectResult okResult && okResult.Value is int id)
                {
                    if (id > 0)
                    {
                        member.ComId = id;
                        community.ComId = id;
                        Models.Member modelMem = member.GetMember();
                        this.context.Members.Add(modelMem);
                        context.SaveChanges();
                        MemberCommunity newMemCom = new MemberCommunity();
                        newMemCom.Member = member;
                        newMemCom.Community = community;
                        return Ok(newMemCom);
                    }
                    else
                    {
                        return BadRequest("Failed to retrieve community ID.");
                    }
                }
                else
                {
                    return BadRequest("Failed to retrieve community ID.");
                }
            }
        }
        catch (Exception ex)
        {

            return BadRequest($"Error: {ex.Message}");
        }
    }

    
    [HttpGet("GetCommunityMemberAccounts")]
    public IActionResult GetCommunityMemberAccounts(int ComId)
    {
        try
        {
            List<DTO.MemberAccount> comMemberAccounts = context.Members
                .Where(m => m.ComId == ComId)
                .Join(
                    context.Accounts,
                    m => m.UserId,
                    a => a.Id,
                    (m, a) => new DTO.MemberAccount
                    {
                        Member = new DTO.Member(m),
                        Account = new DTO.Account(a)
                    }
                )
                .ToList();

            return Ok(comMemberAccounts);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}






