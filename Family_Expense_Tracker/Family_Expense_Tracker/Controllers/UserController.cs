using Family_Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Text;

namespace Family_Expense_Tracker.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = new HttpClient();
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        #region UserLogin
        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userData = new
                    {
                        Email = user.Email,
                        Password = user.Password,
                        Role = user.Role,
                    };
                    var json = JsonConvert.SerializeObject(userData);

                    Console.WriteLine(json);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync($"User/Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var loggedInUser = JsonConvert.DeserializeObject<newUserModel>(data);

                        HttpContext.Session.SetString("JWTToken", loggedInUser.token.ToString());

                        HttpContext.Session.SetString("UserID", loggedInUser.user.UserID.ToString());
                        HttpContext.Session.SetString("Name", loggedInUser.user.Name);
                        HttpContext.Session.SetString("Email", loggedInUser.user.Email);
                        HttpContext.Session.SetString("Password", loggedInUser.user.Password);
                        HttpContext.Session.SetString("FamilyGroupID", loggedInUser.user.FamilyGroupID.ToString());
                        HttpContext.Session.SetString("GroupName", loggedInUser.user.GroupName);
                        HttpContext.Session.SetString("Role", loggedInUser.user.Role);

                        if (loggedInUser.user.Role == "Admin")
                        {
                            return RedirectToAction("AdminDashboard", "Dashboard");
                        }
                        else
                        {
                            return RedirectToAction("MemberDashboard", "Dashboard");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid Email, Password or Role";
                        return View(user);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return View(user);
        }
        #endregion

        #region UserLogout
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("UserLogin", "User");
        }
        #endregion

        #region AdminRegister
        public IActionResult UserRegister()
        {
            return View();
        }

        public async Task<IActionResult> AdminRegister(AdminRegister admin)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(admin);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                response = await _httpClient.PostAsync($"User/Admin", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("UserLogin");
                }
            }
            return View("UserRegister", admin);
        }
        #endregion

        #region UpdateGroupName

        public async Task<IActionResult> UpdateFamilyGroup(int familyGroupID, string groupName)
        {
            if (familyGroupID < 0 || groupName == null)
            {
                TempData["Error"] = "Invalid input data";
                return RedirectToAction("AdminDashboard", "Dashboard");
            }

            HttpResponseMessage response;

            try
            {
                response = await _httpClient.PutAsync($"User/UpdateFamilyGroup?familyGroupID={familyGroupID}&groupName={groupName}", null);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Dashboard");
        }

        #endregion

        #region Add Member
        public async Task<IActionResult> AddMember(int? UserID)
        {
            if (UserID.HasValue)
            {
                var response = await _httpClient.GetAsync($"User/{UserID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var member = JsonConvert.DeserializeObject<AddMember>(data);
                    return View(member);
                }
            }
            return View(new AddMember());
        }

        public async Task<IActionResult> Save(AddMember member)
        {
            if (ModelState.IsValid)
            {
                var memberData = new
                {
                    FamilyGroupID = CommonVariable.FamilyGroupID(),
                    UserID = member.UserID,
                    Name = member.Name,
                    Email = member.Email,
                    Password = member.Password,
                };
                var json = JsonConvert.SerializeObject(memberData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (member.UserID == 0 || member.UserID == null)
                {
                    response = await _httpClient.PostAsync($"User/Member", content);
                }
                else
                {
                    response = await _httpClient.PutAsync($"User/Update/{member.UserID}", content);
                }
                if (response.IsSuccessStatusCode)
                {
                    if (member.UserID == CommonVariable.UserID())
                    {
                        HttpContext.Session.SetString("Name", member.Name);
                    }
                    if (member.UserID > 0)
                    {
                        TempData["UpdateSuccess"] = "Member details updated successfully!";
                    }
                    else
                    {
                        TempData["AddSuccess"] = "Member added successfully!";
                    }
                }
                else
                {
                    if (member.UserID > 0)
                    {
                        TempData["UpdateError"] = "Email address already exist!";
                    }
                    else
                    {
                        TempData["AddError"] = "Email address already exist!";
                    }
                }

                    if (CommonVariable.Role() == "Admin")
                    {
                        return RedirectToAction("AdminDashboard", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("MemberDashboard", "Dashboard");
                    }
            }
            return View("AddMember", member);
        }
        #endregion

        #region Member Delete

        public async Task<IActionResult> MemberDelete(int UserID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"User/RemoveMember/{UserID}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["DeleteSuccess"] = "Member deleted successfully!";
                }
                else
                {
                    TempData["DeleteError"] = "An error occur while delete member!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("AdminDashboard", "Dashboard");
        }

        #endregion
    }
}
