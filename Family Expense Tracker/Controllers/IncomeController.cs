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
    public class IncomeController : ControllerBase
    {

        private readonly IncomeRepository _IncomeRepository;

        public IncomeController(IncomeRepository incomeRepository)
        {
            _IncomeRepository = incomeRepository;
        }

        #region AddIncome
        [HttpPost]
        [Authorize]
        public IActionResult AddIncome(IncomeModel income)
        {
            if (income == null)
            {
                return BadRequest();
            }
            try
            {
                bool isInserted = _IncomeRepository.AddIncome(income);

                if (isInserted)
                {
                    return Ok(new { Status = "Success", Message = "Income Inserted Successfully" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return StatusCode(500, "An error occurred while inserting Income");
        }
        #endregion

        #region UpdateIncome
        [HttpPut("{ID}")]
        [Authorize]
        public IActionResult UpdateIncome(int ID, IncomeModel income)
        {
            if (ID != income.IncomeID || income == null || income.IncomeID <= 0)
            {
                return BadRequest(new { message = "Invalid income details provided" });
            }

            try
            {
                bool isUpdated = _IncomeRepository.UpdateIncome(income);

                if (isUpdated)
                {
                    return Ok(new { Status = "Success", Message = "Income details updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Income not found or no changes made" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region SelectAllIncome
        [HttpGet("GroupWise/{FamilyGroupID}")]
        [Authorize]
        public IActionResult SelectAllIncome(int FamilyGroupID)
        {
            try
            {
                var incomes = _IncomeRepository.SelectAllIncome(FamilyGroupID);

                if (incomes.Count != 0)
                {

                    return Ok(incomes);
                }
                else
                {

                    return NotFound(new { Message = "There is no income in family group" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region SelectIncomeByID
        [HttpGet("{IncomeID}")]
        [Authorize]
        public IActionResult SelectIncomeByID(int IncomeID)
        {
            try
            {
                var income = _IncomeRepository.SelectIncomeByID(IncomeID);

                return Ok(income);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region SelectIncomeByUserID
        [HttpGet("UserWise/{UserID}")]
        [Authorize]
        public IActionResult IncomeByUserID(int UserID)
        {
            try
            {
                var incomes = _IncomeRepository.IncomeByUserID(UserID);
                if (incomes.Count != 0)
                {
                    return Ok(incomes);
                }
                else
                {
                    return NotFound(new { Message = "There is no Income for this user" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region DeleteIncome
        [HttpDelete("{IncomeID}")]
        [Authorize]
        public IActionResult DeleteIncome(int IncomeID)
        {
            if (IncomeID <= 0)
            {
                return BadRequest(new { message = "Invalid Income ID provided" });
            }

            try
            {
                bool isRemoved = _IncomeRepository.DeleteIncome(IncomeID);

                if (isRemoved)
                {
                    return Ok(new { Status = "Success", Message = "Income removed successfully" });
                }
                else
                {
                    return NotFound(new { message = "Income not found or already removed" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion
    }
}
