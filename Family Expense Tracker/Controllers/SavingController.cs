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
    public class SavingController : ControllerBase
    {
        private readonly SavingRepository _savingRepository;

        public SavingController(SavingRepository savingRepository)
        {
            _savingRepository = savingRepository;
        }

        #region AddSaving
        [HttpPost]
        [Authorize]
        public IActionResult AddSaving(SavingModel saving)
        {
            if (saving == null)
            {
                return BadRequest();
            }
            try
            {
                bool isInserted = _savingRepository.AddSaving(saving);

                if (isInserted)
                {
                    return Ok(new { Status = "Success", Message = "saving Inserted Successfully" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return StatusCode(500, "An error occurred while inserting saving");
        }
        #endregion

        #region UpdateSaving
        [HttpPut("{ID}")]
        [Authorize]
        public IActionResult UpdateSaving(int ID, SavingModel saving)
        {
            if (ID != saving.SavingID || saving == null || saving.SavingID <= 0)
            {
                return BadRequest(new { message = "Invalid saving details provided" });
            }

            try
            {
                bool isUpdated = _savingRepository.UpdateSaving(saving);

                if (isUpdated)
                {
                    return Ok(new { Status = "Success", Message = "saving details updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "saving not found or no changes made" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region SelectAllSaving
        [HttpGet("GroupWise/{FamilyGroupID}")]
        [Authorize]
        public IActionResult SelectAllSaving(int FamilyGroupID)
        {
            try
            {
                var savings = _savingRepository.SelectAllSaving(FamilyGroupID);

                if (savings.Count != 0)
                {

                    return Ok(savings);
                }
                else
                {

                    return NotFound(new { Message = "There is no savings in family group" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region SelectSavingByID
        [HttpGet("{SavingID}")]
        [Authorize]
        public IActionResult SelectSavingByID(int SavingID)
        {
            try
            {
                var Savings = _savingRepository.SelectSavingByID(SavingID);

                return Ok(Savings);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region SelectSavingByUserID
        [HttpGet("UserWise/{UserID}")]
        [Authorize]
        public IActionResult SavingByUserID(int UserID)
        {
            try
            {
                var savings = _savingRepository.SavingByUserID(UserID);
                if (savings.Count != 0)
                {
                    return Ok(savings);
                }
                else
                {
                    return NotFound(new { Message = "There is no Savings for this user" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region DeleteSaving
        [HttpDelete("{SavingID}")]
        [Authorize]
        public IActionResult DeleteSaving(int SavingID)
        {
            if (SavingID <= 0)
            {
                return BadRequest(new { message = "Invalid Saving ID provided" });
            }

            try
            {
                bool isRemoved = _savingRepository.DeleteSavings(SavingID);

                if (isRemoved)
                {
                    return Ok(new { Status = "Success", Message = "Saving removed successfully" });
                }
                else
                {
                    return NotFound(new { message = "Saving not found or already removed" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion


        #region UpdateSavingAmount
        [HttpPut("updateAmount/{ID}")]
        [Authorize]
        public IActionResult UpdateSavingAmount(int ID, [FromQuery] decimal SavedAmount , [FromQuery] int UserID)
        {
            if (ID < 0 || SavedAmount < 0)
            {
                return BadRequest(new { message = "Invalid saving details provided" });
            }

            try
            {
                bool isUpdated = _savingRepository.UpdateSavingAmount(ID , SavedAmount , UserID);

                if (isUpdated)
                {
                    return Ok(new { Status = "Success", Message = "saving Amount updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "saving not found or no changes made" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region SavingContribution
        [HttpGet("SavingContribution/{UserID}")]
        [Authorize]
        public IActionResult SavingContribution(int UserID)
        {
            try
            {
                var savings = _savingRepository.SelectSavingContribution(UserID);
                if (savings.Count != 0)
                {
                    return Ok(savings);
                }
                else
                {
                    return NotFound(new { Message = "There is no Savings Contribution for this user" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}
