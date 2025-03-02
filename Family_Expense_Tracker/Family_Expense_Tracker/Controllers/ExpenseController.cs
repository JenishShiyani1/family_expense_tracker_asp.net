using Family_Expense_Tracker.BAL;
using Family_Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Family_Expense_Tracker.Controllers
{
    [CheckAccess]
    public class ExpenseController : Controller
    {
        private IConfiguration _configuration;

        private readonly IExcelExportService _excelExportService;

        private readonly HttpClient _httpClient;

        public ExpenseController(IHttpClientFactory httpClientFactory, IExcelExportService excelExportService)
        {
            _httpClient = new HttpClient();
            _httpClient = httpClientFactory.CreateClient("ApiClient");

            _excelExportService = excelExportService;
        }
        #region ExpenseList
        public async Task<IActionResult> ExpenseList()
        {
            var response = await _httpClient.GetAsync($"Expense/UserWise/{CommonVariable.UserID()}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var expenses = JsonConvert.DeserializeObject<List<ExpenseModel>>(data);
                return View(expenses);
            }
            return View(new List<ExpenseModel>());
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

        #region AddEditExpense
        public async Task<IActionResult> AddExpense(int? expenseID)
        {
            await ExpenseCategoryList();
            if (expenseID.HasValue)
            {
                var response = await _httpClient.GetAsync($"Expense/{expenseID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var expense = JsonConvert.DeserializeObject<ExpenseModel>(data);
                    return View(expense);
                }
            }
            return View(new ExpenseModel());
        }

        public async Task<IActionResult> Save(ExpenseModel expense, bool IsConfirmed)
        {
            if(ModelState.IsValid)
            {
                decimal previousAmount = 0;

                if (expense.ExpenseID > 0)
                {
                    var response0 = await _httpClient.GetAsync($"Expense/{expense.ExpenseID}");
                    if (response0.IsSuccessStatusCode)
                    {
                        var data = await response0.Content.ReadAsStringAsync();
                        var existingExpense = JsonConvert.DeserializeObject<ExpenseModel>(data);
                        previousAmount = existingExpense.Amount;
                    }
                }
                if (!IsConfirmed)
                {
                    var data = new
                    {
                        FamilyGroupID = CommonVariable.FamilyGroupID(),
                        UserID = CommonVariable.UserID(),
                        CategoryID = expense.CategoryID,
                        ExpenseDate = expense.ExpenseDate,
                    };

                    var response1 = await _httpClient.GetAsync($"Budget/RemainingBudget/{data.FamilyGroupID}/{data.UserID}/{data.CategoryID}/{data.ExpenseDate.ToString("yyyy-MM-dd")}");

                    if (response1.IsSuccessStatusCode)
                    {
                        var json1 = await response1.Content.ReadAsStringAsync();
                        var remainingBudget = JsonConvert.DeserializeObject<RemainingBudgetModel>(json1);

                        if (((expense.ExpenseID == 0 || expense.ExpenseID == null) && remainingBudget.RemainingBudget < expense.Amount) || (expense.ExpenseID > 0 && remainingBudget.RemainingBudget < expense.Amount - previousAmount))
                        {
                            TempData["BudgetExceed"] = "true";
                            TempData["ExpenseData"] = JsonConvert.SerializeObject(expense);
                            await ExpenseCategoryList();
                            return View("AddExpense", expense);
                        }
                    }
                }
                var expenseData = new
                {
                    FamilyGroupID = CommonVariable.FamilyGroupID(),
                    UserID = CommonVariable.UserID(),
                    ExpenseID = expense.ExpenseID,
                    CategoryID = expense.CategoryID,
                    Amount = expense.Amount,
                    ExpenseDate = expense.ExpenseDate,
                    Notes = expense.Notes,
                };

                var json = JsonConvert.SerializeObject(expenseData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (expense.ExpenseID == 0 || expense.ExpenseID == null)
                {
                    response = await _httpClient.PostAsync($"Expense", content);
                }
                else
                {
                    response = await _httpClient.PutAsync($"Expense/{expense.ExpenseID}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    if (expense.ExpenseID > 0)
                    {
                        TempData["UpdateSuccess"] = "Expense updated successfully!";
                    }
                    else
                    {
                        TempData["AddSuccess"] = "Expense added successfully!";
                    }
                    return RedirectToAction("ExpenseList");
                }
                else
                {
                    if (expense.ExpenseID > 0)
                    {
                        TempData["UpdateError"] = "Error while update expense!";
                    }
                    else
                    {
                        TempData["AddError"] = "Error while add expense!";
                    }
                    return RedirectToAction("ExpenseList");
                }
            }
            await ExpenseCategoryList();
            return View("AddExpense", expense);
        }

        #endregion

        #region DeleteExpense
        public async Task<IActionResult> Delete(int expenseID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Expense/{expenseID}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["DeleteSuccess"] = "Expense deleted successfully!";
                }
                else
                {
                    TempData["DeleteError"] = "An error occur while delete expense!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("ExpenseList");
        }
        #endregion

        #region GroupWiseExpense
        public async Task<IActionResult> ExpenseListGroupWise()
        {
            var response = await _httpClient.GetAsync($"Expense/GroupWise/{CommonVariable.FamilyGroupID()}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var expenses = JsonConvert.DeserializeObject<List<ExpenseModel>>(data);
                return View(expenses);
            }
            return View(new List<ExpenseModel>());
        }
        #endregion

        #region ExportToExcelUserExpense
        public async Task<IActionResult> ExportToExcelUserExpense()
        {
            return await _excelExportService.ExportToExcelAsync("Expense", "UserWise", CommonVariable.UserID(), "UsersExpense");
        }
        #endregion

        #region ExportToExcelGroupExpense
        public async Task<IActionResult> ExportToExcelGroupExpense()
        {
            return await _excelExportService.ExportToExcelAsync("Expense", "GroupWise", CommonVariable.FamilyGroupID(), "GroupExpense");
        }
        #endregion
    }
}
