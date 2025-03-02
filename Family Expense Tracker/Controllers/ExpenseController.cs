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
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseRepository _expenseRepository;

        public ExpenseController(ExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        #region AddExpense
        [HttpPost]
        [Authorize]
        public IActionResult AddExpense(ExpenseModel expense)
        {
            if (expense == null)
            {
                return BadRequest();
            }
            try
            {
                bool isInserted = _expenseRepository.AddExpense(expense);

                if (isInserted)
                {
                    return Ok(new { Status = "Success", Message = "Expense Inserted Successfully" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
            return StatusCode(500, "An error occurred while inserting Expense");
        }
        #endregion

        #region UpdateExpense
        [HttpPut("{ID}")]
        [Authorize]
        public IActionResult UpdateExpense(int ID, ExpenseModel expense)
        {
            if (ID != expense.ExpenseID || expense == null || expense.ExpenseID <= 0)
            {
                return BadRequest(new { message = "Invalid expense details provided" });
            }

            try
            {
                bool isUpdated = _expenseRepository.UpdateExpense(expense);

                if (isUpdated)
                {
                    return Ok(new { Status = "Success", Message = "Expense details updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Expense not found or no changes made" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region SelectAllExpense
        [HttpGet("GroupWise/{FamilyGroupID}")]
        [Authorize]
        public IActionResult SelectAllExpense(int FamilyGroupID)
        {
            try
            {
                var expenses = _expenseRepository.SelectAllExpense(FamilyGroupID);

                if (expenses.Count != 0)
                {

                    return Ok(expenses);
                }
                else
                {

                    return NotFound(new { Message = "There is no Expenses in family group" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region SelectExpenseByID
        [HttpGet("{ExpenseID}")]
        [Authorize]
        public IActionResult SelectExpenseByID(int ExpenseID)
        {
            try
            {
                var expense = _expenseRepository.SelectExpenseByID(ExpenseID);

                return Ok(expense);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region SelectExpenseByUserID
        [HttpGet("UserWise/{UserID}")]
        [Authorize]
        public IActionResult ExpenseByUserID(int UserID)
        {
            try
            {
                var expenses = _expenseRepository.ExpenseByUserID(UserID);
                if (expenses.Count != 0)
                {
                    return Ok(expenses);
                }
                else
                {
                    return NotFound(new { Message = "There is no Expense for this user" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region DeleteExpense
        [HttpDelete("{ExpenseID}")]
        [Authorize]
        public IActionResult DeleteExpense(int ExpenseID)
        {
            if (ExpenseID <= 0)
            {
                return BadRequest(new { message = "Invalid Expense ID provided" });
            }

            try
            {
                bool isRemoved = _expenseRepository.DeleteExpense(ExpenseID);

                if (isRemoved)
                {
                    return Ok(new { Status = "Success", Message = "Expense removed successfully" });
                }
                else
                {
                    return NotFound(new { message = "Expense not found or already removed" });
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
