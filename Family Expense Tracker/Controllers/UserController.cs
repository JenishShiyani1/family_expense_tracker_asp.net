using Family_Expense_Tracker.BAL;
using Family_Expense_Tracker.Data;
using Family_Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Family_Expense_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserController(UserRepository userRepository, IHttpContextAccessor contextAccessor)
        {
            this._userRepository = userRepository;
            _contextAccessor = contextAccessor;
        }

        #region AddAdmin
        [HttpPost("Admin")]
        public IActionResult AddAdmin(AdminRegister user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            bool isInserted = _userRepository.AddAdmin(user);

            if(isInserted)
            {
                return Ok(new {Status = "Success" , Message = "Admin Inserted Successfully" });
            }

            return StatusCode(500, "An error occurred while inserting Admin");
        }
        #endregion

        #region AddMember
        [HttpPost("Member")]

        [CheckAccess]
        [Authorize]
        public IActionResult AddMember(AddMember user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                bool isInserted = _userRepository.AddMember(user);

                if (isInserted)
                {
                    return Ok(new { Status = "Success", Message = "Member Inserted Successfully" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return StatusCode(500, "An error occurred while inserting Member");
        }
        #endregion

        #region Admin&MemberLogin
        [HttpPost("Login")]
        public IActionResult AdminMemberLogin(UserLoginModel user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Role))
            {
                return BadRequest(new {message = "Please enter email, password or role"});
            }

            try
            {
                var User = _userRepository.AdminMemberLogin(user);

                var commonCookieOptions = new CookieOptions
                {
                    Secure = true, // Set to true for production (ensure you're using HTTPS)
                    SameSite = SameSiteMode.None, // Necessary for cross-origin requests
                    Expires = DateTime.UtcNow.AddDays(1) // Set expiration as needed
                };

                // Set the 'isLogin' cookie
                _contextAccessor.HttpContext!.Response.Cookies.Append("isLogin", "true", commonCookieOptions);

                return Ok(User);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message =  ex.Message});
            }
        }
        #endregion

        #region FamilyGroupWiseMember
        [HttpGet("GroupMember/{AdminID}")]
        [Authorize]

        [CheckAccess]
        public IActionResult GroupWiseMember(int AdminID)
        {
            if (AdminID == null)
            {
                return BadRequest();
            }

            try
            {
                var members = _userRepository.GroupWiseMember(AdminID);

                if(members.Count != 0)
                {

                    return Ok(members);
                }
                else
                {

                    return NotFound(new {Message = "There is no member in family group" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region MemberDashboard
        [HttpGet("Dashboard/{MemberUserID}")]
        [Authorize]
        public IActionResult GetMemberDashboard(int MemberUserID)
        {
            if (MemberUserID <= 0)
            {
                return BadRequest(new { message = "Invalid Member User ID" });
            }

            try
            {
                var dashboard = _userRepository.MemberAdminSelect(MemberUserID);

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region UpdateUserDetails
        [HttpPut("Update/{ID}")]
        [Authorize]

        [CheckAccess]
        public IActionResult UpdateUserDetails(int ID , AddMember user)
        {
            if (ID != user.UserID || !ModelState.IsValid || user.UserID <= 0)
            {
                return BadRequest(new { message = "Invalid user details provided" });
            }

            try
            {
                bool isUpdated = _userRepository.UpdateUserDetails(user);

                if (isUpdated)
                {
                    return Ok(new { Status = "Success", Message = "User details updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "User not found or no changes made" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region UpdateFamilyGroupName
        [HttpPut("UpdateFamilyGroup")]
        [Authorize]

        [CheckAccess]
        public IActionResult UpdateFamilyGroupName(int familyGroupID, string groupName)
        {
            if (familyGroupID <= 0 || string.IsNullOrEmpty(groupName))
            {
                return BadRequest(new { message = "Invalid Family Group ID or Group Name provided" });
            }

            try
            {
                bool isUpdated = _userRepository.UpdateFamilyGroupName(familyGroupID, groupName);

                if (isUpdated)
                {
                    return Ok(new { Status = "Success", Message = "Family Group Name updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Family Group not found or no changes made" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region RemoveMember
        [HttpDelete("RemoveMember/{memberUserID}")]
        [Authorize]

        [CheckAccess]
        public IActionResult RemoveMember(int memberUserID)
        {
            if (memberUserID <= 0)
            {
                return BadRequest(new { message = "Invalid Member User ID provided" });
            }

            try
            {
                bool isRemoved = _userRepository.RemoveMember(memberUserID);

                if (isRemoved)
                {
                    return Ok(new { Status = "Success", Message = "Member removed successfully" });
                }
                else
                {
                    return NotFound(new { message = "Member not found or already removed" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region SelectUserByID

        [HttpGet("{UserID}")]
        [Authorize]

        [CheckAccess]
        public IActionResult SelectUserByID(int UserID)
        {
            try
            {
                var user = _userRepository.SelectUserByID(UserID);

                return Ok(user);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}
