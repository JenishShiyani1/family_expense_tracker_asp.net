using Family_Expense_Tracker.BAL;
using Family_Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Family_Expense_Tracker.Controllers
{
    [CheckAccess]
    public class IncomeController : Controller
    {
        private IConfiguration _configuration;

        private readonly IExcelExportService _excelExportService;

        private readonly HttpClient _httpClient;

        public IncomeController(IHttpClientFactory httpClientFactory, IExcelExportService excelExportService)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _excelExportService = excelExportService;
        }

        #region IncomeList
        public async Task<IActionResult> IncomeList()
        {
            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("", "");
            var response = await _httpClient.GetAsync($"Income/UserWise/{CommonVariable.UserID()}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var incomes = JsonConvert.DeserializeObject<List<IncomeModel>>(data);
                return View(incomes);
            }
            return View(new List<IncomeModel>());
        }
        #endregion

        #region IncomeCategoryDropDown
        public async Task IncomeCategoryList()
        {
            var categories = new List<CategoryDropDown>();

            var CategoryResponse = await _httpClient.GetAsync($"Category/Income/{CommonVariable.FamilyGroupID()}");

            if (CategoryResponse.IsSuccessStatusCode)
            {
                var data = await CategoryResponse.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<CategoryDropDown>>(data);
                ViewBag.CategoryList = categories;
            }
        }
        #endregion

        #region AddEditIncome
        public async Task<IActionResult> AddIncome(int? incomeID)
        {
            await IncomeCategoryList();
            if (incomeID.HasValue)
            {
                var response = await _httpClient.GetAsync($"Income/{incomeID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var income = JsonConvert.DeserializeObject<IncomeModel>(data);
                    return View(income);
                }
            }
            return View(new IncomeModel());
        }

        public async Task<IActionResult> Save(IncomeModel income)
        {
            if(income.CategoryID <= 0)
            {
                ModelState.AddModelError("CategoryID" , "A valid Category ID is required");
            }
            if (ModelState.IsValid)
            {
                var incomeData = new
                {
                    FamilyGroupID = CommonVariable.FamilyGroupID(),
                    UserID = CommonVariable.UserID(),
                    IncomeID = income.IncomeID,
                    CategoryID = income.CategoryID,
                    Amount = income.Amount,
                    IncomeDate = income.IncomeDate,
                    Notes = income.Notes,
                };
                var json = JsonConvert.SerializeObject(incomeData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (income.IncomeID == 0 || income.IncomeID == null)
                {
                    response = await _httpClient.PostAsync($"Income", content);
                }
                else
                {
                    response = await _httpClient.PutAsync($"Income/{income.IncomeID}", content);
                }
                if (response.IsSuccessStatusCode)
                {
                    if(income.IncomeID > 0)
                    {
                        TempData["UpdateSuccess"] = "Income updated successfully!";
                    }
                    else
                    {
                        TempData["AddSuccess"] = "Income added successfully!";
                    }
                    return RedirectToAction("IncomeList");
                }
                else
                {
                    if (income.IncomeID > 0)
                    {
                        TempData["UpdateError"] = "Error while update income!";
                    }
                    else
                    {
                        TempData["AddError"] = "Error while add income!";
                    }
                    return RedirectToAction("IncomeList");
                }
            }
            await IncomeCategoryList();
            return View("AddIncome", income);
        }
        #endregion

        #region DeleteIncome
        public async Task<IActionResult> Delete(int incomeID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Income/{incomeID}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["DeleteSuccess"] = "Income deleted successfully!";
                }
                else
                {
                    TempData["DeleteError"] = "An error occur while delete income!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("IncomeList");
        }
        #endregion

        #region GroupWiseIncome
        public async Task<IActionResult> IncomeListGroupWise()
        {
            var response = await _httpClient.GetAsync($"Income/GroupWise/{CommonVariable.FamilyGroupID()}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var incomes = JsonConvert.DeserializeObject<List<IncomeModel>>(data);
                return View(incomes);
            }
            return View(new List<IncomeModel>());
        }
        #endregion

        #region ExportToExcelUserIncome
        public async Task<IActionResult> ExportToExcelUserIncome()
        {
            return await _excelExportService.ExportToExcelAsync("Income","UserWise", CommonVariable.UserID() ,"UsersIncome");
        }
        #endregion

        #region ExportToExcelGroupIncome
        public async Task<IActionResult> ExportToExcelGroupIncome()
        {
            return await _excelExportService.ExportToExcelAsync("Income", "GroupWise", CommonVariable.FamilyGroupID() , "GroupIncome");
        }
        #endregion
    }
}
