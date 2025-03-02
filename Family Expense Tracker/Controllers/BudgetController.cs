using Family_Expense_Tracker.BAL;
using Family_Expense_Tracker.Data;
using Family_Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Family_Expense_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [CheckAccess]
    public class BudgetController : ControllerBase
    {
        private readonly BudgetRepository _budgetRepository;
        public BudgetController(BudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        #region AddBudget
        [HttpPost]
        [Authorize]
        public IActionResult AddBudget(BudgetModel budget)
        {
            if (budget == null)
            {
                return BadRequest();
            }

            bool isInserted = _budgetRepository.AddBudget(budget);

            try
            {
                if (isInserted)
                {
                    return Ok(new { Status = "Success", Message = "budget Inserted Successfully" });
                }
                else
                {
                    return NotFound(new {message = "Budget is already exist"});
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region UpdateBudget
        [HttpPut("{ID}")]
        [Authorize]
        public IActionResult UpdateBudget(int ID, BudgetModel budget)
        {
            if (ID != budget.BudgetID || budget == null || budget.BudgetID <= 0)
            {
                return BadRequest(new { message = "Invalid budget details provided" });
            }

            try
            {
                bool isUpdated = _budgetRepository.UpdateBudget(budget);

                if (isUpdated)
                {
                    return Ok(new { Status = "Success", Message = "budget details updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "budget not found or no changes made" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region SelectAllBudget
        [HttpGet("GroupWise/{FamilyGroupID}")]
        [Authorize]
        public IActionResult SelectAllBudget(int FamilyGroupID)
        {
            try
            {
                var budgets = _budgetRepository.SelectAllBudget(FamilyGroupID);

                if (budgets.Count != 0)
                {

                    return Ok(budgets);
                }
                else
                {

                    return NotFound(new { Message = "There is no Budgets in family group" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region SelectBudgetByID
        [HttpGet("{BudgetID}")]
        [Authorize]
        public IActionResult SelectBudgetByID(int BudgetID)
        {
           
            try
            {
                var budget = _budgetRepository.SelectBudgetByID(BudgetID);

                return Ok(budget);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region SelectBudgetByUserID

        [HttpGet("UserWise/{UserID}")]
        [Authorize]
        public IActionResult BudgetByUserID(int UserID)
        {
            try
            {
                var budgets = _budgetRepository.BudgetByUserID(UserID);
                if (budgets.Count != 0)
                {
                    return Ok(budgets);
                }
                else
                {
                    return NotFound(new { Message = "There is no Budgets for this user" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region DeleteBudget
        [HttpDelete("{BudgetID}")]
        [Authorize]
        public IActionResult DeleteBudget(int BudgetID)
        {
            if (BudgetID <= 0)
            {
                return BadRequest(new { message = "Invalid Budget ID provided" });
            }

            try
            {
                bool isRemoved = _budgetRepository.DeleteBudget(BudgetID);

                if (isRemoved)
                {
                    return Ok(new { Status = "Success", Message = "Budget removed successfully" });
                }
                else
                {
                    return NotFound(new { message = "Budget not found or already removed" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region GetRemainingBudget
        [HttpGet("RemainingBudget/{FamilyGroupID}/{UserID}/{CategoryID}/{ExpenseDate}")]
        [Authorize]
        public IActionResult GetRemainingBudget(int FamilyGroupID,int UserID,int CategoryID , string ExpenseDate)
        {
            if (FamilyGroupID <= 0 || UserID <= 0 || CategoryID <= 0)
            {
                return BadRequest("Invalid parameters.");
            }

            var remainingBudget = _budgetRepository.GetRemainingBudget(FamilyGroupID , UserID , CategoryID , ExpenseDate);

            if (remainingBudget.RemainingBudget != null)
            {
                return Ok(remainingBudget);
            }
            return StatusCode(500, "There is no budget for this category");
        }
        #endregion
    }
}
