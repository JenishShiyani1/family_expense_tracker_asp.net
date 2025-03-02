using Family_Expense_Tracker.BAL;
using Family_Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Family_Expense_Tracker.Controllers
{
    [CheckAccess]
    public class BudgetController : Controller
    {
        private IConfiguration _configuration;

        private readonly IExcelExportService _excelExportService;

        private readonly HttpClient _httpClient;

        public BudgetController(IHttpClientFactory httpClientFactory, IExcelExportService excelExportService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _excelExportService = excelExportService;
        }
        #region BudgetList
        public async Task<IActionResult> BudgetList()
        {
            var response = await _httpClient.GetAsync($"Budget/UserWise/{CommonVariable.UserID()}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var budgets = JsonConvert.DeserializeObject<List<BudgetModel>>(data);
                return View(budgets);
            }
            return View(new List<BudgetModel>());
        }
        #endregion

        #region ExpenseCategoryList
        public async Task ExpenseCategoryList()
        {
            var categories = new List<CategoryDropDown>();

            var CategoryResponse = await _httpClient.GetAsync($"Category/Expense/{CommonVariable.FamilyGroupID()}");

            if (CategoryResponse.IsSuccessStatusCode)
            {
                var data = await CategoryResponse.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<CategoryDropDown>>(data);
                ViewBag.CategoryList = categories;
            }
        }
        #endregion

        #region AddEditBudget
        public async Task<IActionResult> AddBudget(int? budgetID)
        {
            await ExpenseCategoryList();
            if (budgetID.HasValue)
            {
                var response = await _httpClient.GetAsync($"Budget/{budgetID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var budget = JsonConvert.DeserializeObject<BudgetModel>(data);
                    return View(budget);
                }
            }
            return View(new BudgetModel());
        }

        public async Task<IActionResult> Save(BudgetModel budget)
        {
            if (budget.CategoryID <= 0)
            {
                ModelState.AddModelError("CategoryID", "A valid Category ID is required");
            }
            if (ModelState.IsValid)
            {
                var budgetData = new
                {
                    FamilyGroupID = CommonVariable.FamilyGroupID(),
                    UserID = CommonVariable.UserID(),
                    BudgetID = budget.BudgetID,
                    CategoryID = budget.CategoryID,
                    TotalBudget = budget.TotalBudget,
                    EndDate = budget.EndDate,
                };
                var json = JsonConvert.SerializeObject(budgetData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (budget.BudgetID == 0 || budget.BudgetID == null)
                {
                    response = await _httpClient.PostAsync($"Budget", content);
                }
                else
                {
                    response = await _httpClient.PutAsync($"Budget/{budget.BudgetID}", content);
                }
                if (response.IsSuccessStatusCode)
                {
                    if (budget.BudgetID > 0)
                    {
                        TempData["UpdateSuccess"] = "Budget updated successfully!";
                    }
                    else
                    {
                        TempData["AddSuccess"] = "Budget added successfully!";
                    }
                    return RedirectToAction("BudgetList");
                }
                else
                {
                    TempData["BudgetExist"] = "true";
                    return RedirectToAction("BudgetList");
                }
            }
            await ExpenseCategoryList();
            return View("AddBudget", budget);
        }
        #endregion

        #region DeleteBudget
        public async Task<IActionResult> Delete(int budgetID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Budget/{budgetID}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["DeleteSuccess"] = "Budget deleted successfully!";
                }
                else
                {
                    TempData["DeleteError"] = "An error occur while delete budget!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("BudgetList");
        }
        #endregion

        #region GroupWiseBudget
        [AdminCheckAccess]
        public async Task<IActionResult> BudgetListGroupWise()
        {
            var response = await _httpClient.GetAsync($"Budget/GroupWise/{CommonVariable.FamilyGroupID()}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var budgets = JsonConvert.DeserializeObject<List<BudgetModel>>(data);
                return View(budgets);
            }
            return View(new List<BudgetModel>());
        }
        #endregion

        #region ExportToExcelUserBudget
        public async Task<IActionResult> ExportToExcelUserBudget()
        {
            return await _excelExportService.ExportToExcelAsync("Budget", "UserWise", CommonVariable.UserID(), "UsersBudget");
        }
        #endregion

        #region ExportToExcelGroupBudget
        public async Task<IActionResult> ExportToExcelGroupBudget()
        {
            return await _excelExportService.ExportToExcelAsync("Budget", "GroupWise", CommonVariable.FamilyGroupID(), "GroupBudget");
        }
        #endregion
    }
}
