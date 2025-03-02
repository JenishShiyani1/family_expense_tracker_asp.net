using Family_Expense_Tracker.BAL;
using Family_Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Family_Expense_Tracker.Controllers
{
    [CheckAccess]
    public class SavingController : Controller
    {
        private IConfiguration _configuration;

        private readonly IExcelExportService _excelExportService;

        private readonly HttpClient _httpClient;

        public SavingController(IHttpClientFactory httpClientFactory, IExcelExportService excelExportService)
        {
            _httpClient = new HttpClient();
            _httpClient = httpClientFactory.CreateClient("ApiClient");

            _excelExportService = excelExportService;
        }

        #region SavingListByUser
        public async Task<IActionResult> SavingList()
        {
            var response = await _httpClient.GetAsync($"Saving/UserWise/{CommonVariable.UserID()}");

            if(response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var savings = JsonConvert.DeserializeObject<List<SavingModel>>(data);
                return View(savings);
            }
            return View(new List<SavingModel>());
        }
        #endregion

        #region AddEditSaving
        public async Task<IActionResult> AddSaving(int? savingID)
        {
            if (savingID.HasValue)
            {
                var response = await _httpClient.GetAsync($"Saving/{savingID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var saving = JsonConvert.DeserializeObject<SavingModel>(data);
                    return View(saving);
                }
            }
            return View(new SavingModel());
        }

        public async Task<IActionResult> Save(SavingModel saving)
        {
            if (ModelState.IsValid)
            {
                var savingData = new
                {
                    FamilyGroupID = CommonVariable.FamilyGroupID(),
                    UserID = CommonVariable.UserID(),
                    SavingID = saving.SavingID,
                    SavingName = saving.SavingName,
                    TargetedAmount = saving.TargetedAmount,
                    DeadLine = saving.DeadLine,
                };
                var json = JsonConvert.SerializeObject(savingData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (saving.SavingID == 0 || saving.SavingID == null)
                {
                    response = await _httpClient.PostAsync($"Saving", content);
                }
                else
                {
                    response = await _httpClient.PutAsync($"Saving/{saving.SavingID}", content);
                }
                if (response.IsSuccessStatusCode)
                {
                    if (saving.SavingID > 0)
                    {
                        TempData["UpdateSuccess"] = "Saving updated successfully!";
                    }
                    else
                    {
                        TempData["AddSuccess"] = "Saving added successfully!";
                    }
                    return RedirectToAction("SavingList");
                }
                else
                {
                    if (saving.SavingID > 0)
                    {
                        TempData["UpdateError"] = "Error while update saving!";
                    }
                    else
                    {
                        TempData["AddError"] = "Error while add saving!";
                    }
                    return RedirectToAction("CategoryList");
                }
            }
            return View("AddSaving", saving);
        }
        #endregion

        #region DeleteSaving
        [HttpGet]
        public async Task<IActionResult> Delete(int savingID)
        {
            var response = await _httpClient.DeleteAsync($"Saving/{savingID}");
            if (response.IsSuccessStatusCode)
            {
                TempData["DeleteSuccess"] = "Saving deleted successfully!"; 
            }
            else
            {
                TempData["DeleteError"] = "An error occur while delete saving!";
            }
            return RedirectToAction("SavingList");
        }
        #endregion

        #region GroupWiseSaving
        public async Task<IActionResult> SavingListGroupWise()
        {
            var response = await _httpClient.GetAsync($"Saving/GroupWise/{CommonVariable.FamilyGroupID()}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var savings = JsonConvert.DeserializeObject<List<SavingModel>>(data);
                return View(savings);
            }
            return View(new List<SavingModel>());
        }
        #endregion

        #region UpdateSavedAmount
        public async Task<IActionResult> UpdateSavedAmount(int savingID, decimal savedAmount , bool flag)
        {
            if (savingID <= 0 || savedAmount <= 0)
            {
                TempData["Error"] = "Invalid input data.";
            }

            var savingData = new
            {
                SavingID = savingID,
                SavedAmount = savedAmount
            };

            HttpResponseMessage response;

            try
            {
                response = await _httpClient.PutAsync($"Saving/updateAmount/{savingID}?SavedAmount={savedAmount}&UserID={CommonVariable.UserID()}" , null);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            if(flag == true)
            {
                return RedirectToAction("SavingList");
            }
            else
            {
                return RedirectToAction("SavingListGroupWise");
            }
        }
        #endregion

        #region ExportToExcelUserSaving
        public async Task<IActionResult> ExportToExcelUserSaving()
        {
            return await _excelExportService.ExportToExcelAsync("Saving", "UserWise", CommonVariable.UserID() ,"UsersSaving");
        }
        #endregion

        #region ExportToExcelGroupSaving
        public async Task<IActionResult> ExportToExcelGroupSaving()
        {
            return await _excelExportService.ExportToExcelAsync("Saving", "GroupWise", CommonVariable.FamilyGroupID() ,"GroupSaving");
        }
        #endregion

        #region SavingContributionListByUser
        public async Task<IActionResult> SavingContributionList()
        {
            var response = await _httpClient.GetAsync($"Saving/SavingContribution/{CommonVariable.UserID()}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var savings = JsonConvert.DeserializeObject<List<SavingContribution>>(data);
                return View(savings);
            }
            return View(new List<SavingContribution>());
        }
        #endregion
    }
}
