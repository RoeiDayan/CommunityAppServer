﻿using Microsoft.AspNetCore.Mvc;
using CommunityAppServer.Models;
using Microsoft.EntityFrameworkCore;
using CommunityAppServer.DTO;
using Azure.Core;

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
                .Where(n => n.ComId == ComId) // Directly filter by ComId
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
            .Select(r => new DTO.Report(r))
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

    [HttpPost("CreateNotice")]
    public IActionResult CreateNotice([FromBody] DTO.Notice notice)
    {
        try
        {
            Models.Notice modelsNotice = notice.GetNotice();
            if (modelsNotice == null)
            {
                return BadRequest("Invalid notice data.");
            }

            context.Notices.Add(modelsNotice);

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
            bool IsCodeValid = this.context.Communities.Any(c => c.ComCode == s);
            if (IsCodeValid)
            {
                int ID = this.context.Communities.Where(c => c.ComCode == s).Select(c => c.ComId).FirstOrDefault();
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


    [HttpGet("GetApprovedCommunityMemberAccounts")]
    public IActionResult GetApprovedCommunityMemberAccounts(int ComId)
    {
        try
        {
            List<DTO.MemberAccount> comMemberAccounts = context.Members
                .Where(m => m.ComId == ComId && m.IsApproved == true) // Only approved members
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

    //[HttpGet("GetCommunityTenantRoom")]
    //public IActionResult GetCommunityTenantRoom(int comId)
    //{
    //    try
    //    {
    //        var tenantRoom = context.TenantRooms
    //            .FirstOrDefault(tr => tr.ComId == comId);

    //        if (tenantRoom == null)
    //            return NotFound($"No tenant room found for community ID {comId}");

    //        return Ok(new DTO.TenantRoom(tenantRoom));
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}



    [HttpGet("GetMemberAccount")]
    public IActionResult GetMemberAccount(int UserId, int ComId)
    {
        try
        {
            var member = context.Members
                .FirstOrDefault(m => m.UserId == UserId && m.ComId == ComId);

            if (member == null)
                return NotFound($"No member found with User ID {UserId} in Community {ComId}");

            var account = context.Accounts.FirstOrDefault(a => a.Id == UserId);
            if (account == null)
                return NotFound($"No account found with ID {UserId}");

            return Ok(new DTO.MemberAccount
            {
                Member = new DTO.Member(member),
                Account = new DTO.Account(account)
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetSelectCommunityMemberAccounts")]
    public IActionResult GetSelectCommunityMemberAccounts(int ComId, bool ApprovedStat)
    {
        try
        {
            List<DTO.MemberAccount> comMemberAccounts = context.Members
                .Where(m => m.ComId == ComId && m.IsApproved == ApprovedStat)
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

    [HttpPost("UpdateMember")]
    public IActionResult UpdateMember([FromBody] DTO.Member member)
    {
        var mem = context.Members
            .FirstOrDefault(m => m.ComId == member.ComId && m.UserId == member.UserId);

        if (mem == null)
        {
            return BadRequest("No member found");
        }

        // Update fields
        mem.Role = member.Role;
        mem.Balance = member.Balance;
        mem.UnitNum = member.UnitNum;
        mem.IsLiable = member.IsLiable;
        mem.IsResident = member.IsResident;
        mem.IsManager = member.IsManager;
        mem.IsProvider = member.IsProvider;
        mem.IsApproved = member.IsApproved;

        try
        {
            context.SaveChanges();
            return Ok("Member updated successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error updating member: {ex.Message}");
        }
    }
    [HttpDelete("RemoveMember")]
    public IActionResult RemoveMember(int ComId, int UserId)
    {
        try
        {
            var member = context.Members
                .FirstOrDefault(m => m.ComId == ComId && m.UserId == UserId);

            if (member == null)
            {
                return NotFound("Member not found");
            }

            // Remove the member from the community
            context.Members.Remove(member);
            context.SaveChanges();

            return Ok("Member removed successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error removing member: {ex.Message}");
        }
    }

    [HttpPost("CreateRoomRequest")]
    public IActionResult CreateRoomRequest([FromBody] DTO.RoomRequest req)
    {
        try
        {
            Models.RoomRequest modelReq = req.GetRoomRequest(); // Assuming you have a converter method
            if (modelReq == null)
                return BadRequest("Invalid data.");

            context.RoomRequests.Add(modelReq);
            int changes = context.SaveChanges();

            return Ok(changes > 0);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }


    [HttpGet("GetSelectRoomRequests")]
    public IActionResult GetSelectRoomRequests(int ComId, bool IsApproved)
    {
        try
        {
            List<DTO.RoomRequest> Requests = context.RoomRequests
                .Where(rr => rr.ComId == ComId && rr.IsApproved == IsApproved)
                .Select(rr => new DTO.RoomRequest(rr))
                .ToList();

            return Ok(Requests);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetAllRoomRequests")]
    public IActionResult GetAllRoomRequests(int ComId)
    {
        try
        {
            List<DTO.RoomRequest> Requests = context.RoomRequests
                .Where(rr => rr.ComId == ComId)
                .Select(rr => new DTO.RoomRequest(rr))
                .ToList();

            return Ok(Requests);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("UpdateRoomRequest")]
    public IActionResult UpdateRoomRequest([FromBody] DTO.RoomRequest roomRequest)
    {
        try
        {
            // Find the existing room request
            Models.RoomRequest? existingRequest = context.RoomRequests.FirstOrDefault(rr => rr.RequestId == roomRequest.RequestId);

            if (existingRequest == null)
            {
                return NotFound("Room request not found");
            }

            // Update properties
            existingRequest.IsApproved = roomRequest.IsApproved;
            existingRequest.Text = roomRequest.Text;
            existingRequest.StartTime = roomRequest.StartTime;
            existingRequest.EndTime = roomRequest.EndTime;

            // Save changes
            context.SaveChanges();
            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteRoomRequest")]
    public IActionResult DeleteRoomRequest(int id)
    {
        try
        {
            // Find the room request
            Models.RoomRequest? roomRequest = context.RoomRequests.FirstOrDefault(rr => rr.RequestId == id);

            if (roomRequest == null)
            {
                return NotFound("Room request not found");
            }

            // Remove the room request
            context.RoomRequests.Remove(roomRequest);
            context.SaveChanges();
            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeletePastRoomRequests")]
    public IActionResult DeletePastRoomRequests(int comId)
    {
        try
        {
            DateTime now = DateTime.Now;


            var pastRequests = context.RoomRequests
                .Where(r => r.ComId == comId && r.EndTime < now)
                .ToList();

            if (pastRequests.Count == 0)
                return Ok(false); // nothing to delete

            context.RoomRequests.RemoveRange(pastRequests);
            context.SaveChanges();
            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost("CreateExpense")]
    public IActionResult CreateExpense([FromBody] DTO.Expense expense)
    {
        try
        {
            context.Expenses.Add(expense.GetExpense());
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

    [HttpGet("GetCommunityExpenses")]
    public IActionResult GetCommunityExpenses(int ComId)
    {
        try
        {
            List<DTO.Expense> exp = context.Expenses
                .Where(e => e.ComId == ComId)
                .Select(e => new DTO.Expense(e))
                .ToList();
            return Ok(exp);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost("IssuePaymentToAllMembers")]
    public IActionResult IssuePaymentToAllMembers([FromBody] DTO.Payment p)
    {
        try
        {
            var approvedMembers = context.Members
                .Where(m => m.ComId == p.ComId && m.IsApproved == true)
                .ToList();

            if (!approvedMembers.Any())
                return NotFound("No approved members found in this community");

            var payments = new List<Models.Payment>();

            foreach (var member in approvedMembers)
            {
                var dtoCopy = new DTO.Payment
                {
                    ComId = p.ComId,
                    UserId = member.UserId,
                    Amount = p.Amount,
                    Details = p.Details,
                    WasPayed = false,
                    PayFrom = p.PayFrom,
                    PayUntil = p.PayUntil
                };
                payments.Add(dtoCopy.GetPayment()); // Convert to Models.Payment
                RecalculateMemberBalance(member.UserId, member.ComId);

            }

            context.Payments.AddRange(payments);
            context.SaveChanges();

            return Ok($"Payment issued to {payments.Count} approved members");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error issuing payment: {ex.Message}");
        }
    }


    [HttpPost("IssuePaymentToMember")]
    public IActionResult IssuePaymentToMember([FromBody] DTO.Payment p)
    {
        try
        {
            // Verify the member exists and is approved in this community
            var member = context.Members
                .FirstOrDefault(m => m.UserId == p.UserId && m.ComId == p.ComId && m.IsApproved == true);

            if (member == null)
                return NotFound($"No approved member found with User ID {p.UserId} in Community {p.ComId}");


            context.Payments.Add(p.GetPayment());
            context.SaveChanges();
            RecalculateMemberBalance(p.UserId, p.ComId);

            return Ok("Payment issued successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetMemberPayments")]
    public IActionResult GetMemberPayments(int ComId, int UserId)
    {
        try
        {
            List<DTO.Payment> payments =
                context.Payments.Where(p => p.ComId == ComId && p.UserId == UserId)
                .Select(p => new DTO.Payment(p)).ToList();
            return Ok(payments);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    private void RecalculateMemberBalance(int userId, int comId)
    {
        var member = context.Members.FirstOrDefault(m => m.UserId == userId && m.ComId == comId);
        if (member == null)
            return;

        var unpaidPayments = context.Payments
            .Where(p => p.UserId == userId && p.ComId == comId && (p.WasPayed == false || p.WasPayed == null))
            .Sum(p => p.Amount);

        member.Balance = unpaidPayments;
        context.SaveChanges();
    }

    [HttpGet("GetCommunityPayments")]
    public IActionResult GetCommunityPayments(int ComId)
    {
        try
        {
            List<PaymentMemberAccount> paymentsWithMemberAccount = (from payment in context.Payments
                                                                    where payment.ComId == ComId
                                                                    join member in context.Members
                                                                        on new { payment.ComId, payment.UserId }
                                                                        equals new { member.ComId, member.UserId }
                                                                    join account in context.Accounts
                                                                        on member.UserId equals account.Id
                                                                    select new PaymentMemberAccount
                                                                    {
                                                                        Payment = new DTO.Payment(payment),
                                                                        Member = new DTO.Member(member),
                                                                        Account = new DTO.Account(account)
                                                                    }).ToList();

            return Ok(paymentsWithMemberAccount);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteMemberPayment")]
    public IActionResult DeleteMemberPayment(int payId)
    {
        try
        {
            Models.Payment pay = context.Payments
                                 .Where(pay => pay.PaymentId == payId)
                                 .FirstOrDefault();
            if (pay != null)
            {
                context.Payments.Remove(pay);
                context.SaveChanges();
                RecalculateMemberBalance(pay.UserId, pay.ComId);

            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut("UpdatePayment")]
    public IActionResult UpdatePayment([FromBody] DTO.Payment payment)
    {
        try
        {
            Models.Payment? existingPayment = context.Payments.FirstOrDefault(p => p.PaymentId == payment.PaymentId);
            if (existingPayment == null)
            {
                return NotFound("Payment not found");
            }

            existingPayment.WasPayed = payment.WasPayed;
            context.SaveChanges();
            RecalculateMemberBalance(existingPayment.UserId, existingPayment.ComId);

            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteNotice")]
    public IActionResult DeleteNotice(int noticeId)
    {
        try
        {
            Models.Notice n = context.Notices
                                 .Where(n => n.NoticeId == noticeId)
                                 .FirstOrDefault();
            if (n != null)
            {
                context.Notices.Remove(n);
                context.SaveChanges();

            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteReport")]
    public IActionResult DeleteReport(int repId)
    {
        try
        {
            Models.Report r = context.Reports
                                 .Where(r => r.ReportId == repId)
                                 .FirstOrDefault();
            if (r != null)
            {
                context.Reports.Remove(r);
                context.SaveChanges();

            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("UploadProfileImage")]
    public async Task<IActionResult> UploadProfileImageAsync(IFormFile file)
    {
        //Check if who is logged in
        string? userEmail = HttpContext.Session.GetString("loggedInUser");
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized("User is not logged in");
        }

        //Get model user class from DB with matching email. 
        Models.Account? acc = context.GetAccount(userEmail);
        //Clear the tracking of all objects to avoid double tracking
        context.ChangeTracker.Clear();
        if (acc == null)
        {
            return Unauthorized("User is not found in the database");
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded");
        }

        //Check the file extension!
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        string extension = "";
        if (file.FileName.LastIndexOf(".") > 0)
        {
            extension = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
        }

        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest("File sent with non supported extension");
        }

        try
        {
            // Check if file is actually an image before saving
            using (var tempStream = new MemoryStream())
            {
                await file.CopyToAsync(tempStream);
                if (!IsImage(tempStream))
                {
                    return BadRequest("File is not a valid image");
                }
            }

            //Build path in the web root
            string fileName = $"{acc.Id}{extension}";
            string filePath = Path.Combine(this.webHostEnvironment.WebRootPath, "profileImages", fileName);

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Delete existing profile images for this user
            string[] existingFiles = Directory.GetFiles(
                Path.Combine(this.webHostEnvironment.WebRootPath, "profileImages"),
                $"{acc.Id}.*");
            foreach (string existingFile in existingFiles)
            {
                System.IO.File.Delete(existingFile);
            }

            // Save the new file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Update the database with the new profile photo filename
            string virtualPath = GetProfileImageVirtualPath(acc.Id);
            acc.ProfilePhotoFileName = virtualPath;

            // Save changes to database
            context.UpdateAccount(acc); // You'll need to implement this method in your context

            DTO.Account dtoAcc = new DTO.Account(acc);
            dtoAcc.ProfilePhotoFileName = virtualPath;

            return Ok(dtoAcc);
        }
        catch (Exception ex)
        {
            // Log the exception
            return StatusCode(500, "An error occurred while uploading the image");
        }
    }

    private string GetProfileImageVirtualPath(int userId)
    {
        string virtualPath = $"/profileImages/{userId}";
        string path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userId}.png";
        if (System.IO.File.Exists(path))
        {
            virtualPath += ".png";
        }
        else
        {
            path = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{userId}.jpg";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".jpg";
            }
            else
            {
                virtualPath = $"/profileImages/default.png";
            }
        }

        return virtualPath;
    }

    //this function gets a file stream and check if it is an image
    private static bool IsImage(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);

        List<string> jpg = new List<string> { "FF", "D8" };
        List<string> bmp = new List<string> { "42", "4D" };
        List<string> gif = new List<string> { "47", "49", "46" };
        List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
        List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png };

        List<string> bytesIterated = new List<string>();

        for (int i = 0; i < 8; i++)
        {
            string bit = stream.ReadByte().ToString("X2");
            bytesIterated.Add(bit);

            bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
            if (isImage)
            {
                return true;
            }
        }

        return false;
    }

}






