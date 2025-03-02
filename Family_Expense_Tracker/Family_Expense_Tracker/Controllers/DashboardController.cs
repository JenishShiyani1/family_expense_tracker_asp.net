using Family_Expense_Tracker.BAL;
using Family_Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace Family_Expense_Tracker.Controllers
{
    [CheckAccess]
    public class DashboardController : Controller
    {
        private readonly HttpClient _httpClient;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = new HttpClient();
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        #region MemberDashboard
        public async Task<IActionResult> MemberDashboard()
        {
            FinanceDashboardData dashboardData = new FinanceDashboardData
            {
                Counts = new List<DashboardCountsModel>(),
                CategoryWiseExpenses = new List<CategoryWiseExpense>(),
                FamilyMembers = new List<FamilyMember>(),
                FamilyAdmins = new List<FamilyAdmin>(),
                RecentExpenses = new List<RecentExpense>(),
                RecentIncomes = new List<RecentIncome>()
            };

            try
            {
                var response = await _httpClient.GetAsync($"Dashboard/Member?UserID={CommonVariable.UserID()}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var dashboard = JsonConvert.DeserializeObject<FinanceDashboardData>(data);

                    dashboardData = dashboard ?? new FinanceDashboardData();
                    dashboardData.Counts ??= new List<DashboardCountsModel>();
                    dashboardData.CategoryWiseExpenses ??= new List<CategoryWiseExpense>();
                    dashboardData.FamilyMembers ??= new List<FamilyMember>();
                    dashboardData.FamilyAdmins ??= new List<FamilyAdmin>();
                    dashboardData.RecentExpenses ??= new List<RecentExpense>();
                    dashboardData.RecentIncomes ??= new List<RecentIncome>();
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to load dashboard data.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(dashboardData);
        }

        #endregion

        #region AdminDashboard
        [AdminCheckAccess]
        public async Task<IActionResult> AdminDashboard()
        {
            AdminFinanceDashboardData dashboardData = new AdminFinanceDashboardData
            {
                Counts = new List<AdminDashboardCountsModel>(),
                CategoryWiseExpenses = new List<AdminCategoryWiseExpense>(),
                FamilyGroupName = new FamilyGroupName(),
                AdminSideMember = new List<AdminSideMember>(),
                RecentExpenses = new List<AdminRecentExpense>(),
                RecentIncomes = new List<AdminRecentIncome>()
            };
            try
            {
                var response = await _httpClient.GetAsync($"Dashboard/Admin?UserID={CommonVariable.UserID()}&FamilyGroupID={CommonVariable.FamilyGroupID()}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var dashboard = JsonConvert.DeserializeObject<AdminFinanceDashboardData>(data);

                    dashboardData = dashboard ?? new AdminFinanceDashboardData();
                    dashboardData.Counts ??= new List<AdminDashboardCountsModel>();
                    dashboardData.CategoryWiseExpenses ??= new List<AdminCategoryWiseExpense>();
                    dashboardData.FamilyGroupName ??= new FamilyGroupName();
                    dashboardData.AdminSideMember ??= new List<AdminSideMember>();
                    dashboardData.RecentExpenses ??= new List<AdminRecentExpense>();
                    dashboardData.RecentIncomes ??= new List<AdminRecentIncome>();
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to load dashboard data.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(dashboardData);
        }
        #endregion
    }
}
