using Family_Expense_Tracker.BAL;
using Family_Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Family_Expense_Tracker.Controllers
{
    [CheckAccess]
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoryController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = new HttpClient();
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        #region CategoryList
        public async Task<IActionResult> CategoryList()
        {
            var incomeResponse = await _httpClient.GetAsync($"Category/Income/{CommonVariable.FamilyGroupID()}");
            var expenseResponse = await _httpClient.GetAsync($"Category/Expense/{CommonVariable.FamilyGroupID()}");

            var incomeCategories = incomeResponse.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<List<CategoryModel>>(await incomeResponse.Content.ReadAsStringAsync())
                : new List<CategoryModel>();

            var expenseCategories = expenseResponse.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<List<CategoryModel>>(await expenseResponse.Content.ReadAsStringAsync())
                : new List<CategoryModel>();

            var viewModel = new CategoryListViewModel
            {
                IncomeCategories = incomeCategories,
                ExpenseCategories = expenseCategories
            };

            return View(viewModel);
        }
        #endregion

        #region AddEditCategory
        public async Task<IActionResult> AddCategory(int? categoryID)
        {
            if(categoryID.HasValue)
            {
                var response = await _httpClient.GetAsync($"Category/{categoryID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var category = JsonConvert.DeserializeObject<CategoryModel>(data);
                    return View(category);
                }
            }
            return View(new CategoryModel());
        }

        public async Task<IActionResult> Save(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                var categoryData = new
                {
                    CategoryID = category.CategoryID,
                    CategoryName = category.CategoryName,
                    IsIncomeCategory = category.IsIncomeCategory,
                    FamilyGroupID = CommonVariable.FamilyGroupID(),
                    UserID = CommonVariable.UserID(),
                };
                var json = JsonConvert.SerializeObject(categoryData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (category.CategoryID == 0 || category.CategoryID == null)
                {
                    response = await _httpClient.PostAsync($"Category", content);
                }
                else
                {
                    response = await _httpClient.PutAsync($"Category/{category.CategoryID}", content);
                }
                if (response.IsSuccessStatusCode)
                {
                    if (category.CategoryID > 0)
                    {
                        TempData["UpdateSuccess"] = "Category updated successfully!";
                    }
                    else
                    {
                        TempData["AddSuccess"] = "Category added successfully!";
                    }
                    return RedirectToAction("CategoryList");
                }
                else
                {
                    if (category.CategoryID > 0)
                    {
                        TempData["UpdateError"] = "Category is already exist!";
                    }
                    else
                    {
                        TempData["AddError"] = "Category is already exist!";
                    }
                    return RedirectToAction("CategoryList");
                }
            }
            return View("AddCategory", category);
        }
        #endregion

        #region DeleteCategory
        public async Task<IActionResult> Delete(int categoryID)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Category/{categoryID}");

                if(response.IsSuccessStatusCode)
                {
                    TempData["DeleteSuccess"] = "Category deleted successfully!";
                }
                else
                {
                    TempData["deleteEx"] = "Cannot delete Category due to related records in the database.";
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("CategoryList");
        }
        #endregion
    }
}
