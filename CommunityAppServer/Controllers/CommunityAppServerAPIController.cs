using Microsoft.AspNetCore.Mvc;
using CommunityAppServer.Models;
using Microsoft.EntityFrameworkCore;

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
            List<Notice> comNotices = context.Notices
                .Where(n => n.Coms.Any(c => c.ComId == ComId))
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

            List<Report> comReports = context.Reports
            .Where(r => r.ComId == ComId)
            .ToList();


            return Ok(comReports);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}





