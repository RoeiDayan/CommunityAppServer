using Microsoft.AspNetCore.Mvc;
using CommunityAppServer.Models;

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
    //[HttpPost("login")]
    //public IActionResult Login([FromBody] DTO.LoginInfo loginDto)
    //{
    //    try
    //    {
    //        HttpContext.Session.Clear(); //Logout any previous login attempt

    //        //Get model user class from DB with matching email. 
    //        Models.Account? modelsUser = context.GetUser(loginDto.Email);

    //        //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
    //        if (modelsUser == null || modelsUser.UserPassword != loginDto.Password)
    //        {
    //            return Unauthorized();
    //        }

    //        //Login suceed! now mark login in session memory!
    //        HttpContext.Session.SetString("loggedInUser", modelsUser.UserEmail);

    //        DTO.AppUser dtoUser = new DTO.AppUser(modelsUser);
    //        dtoUser.ProfileImagePath = GetProfileImageVirtualPath(dtoUser.Id);
    //        return Ok(dtoUser);
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }

    //}


}
