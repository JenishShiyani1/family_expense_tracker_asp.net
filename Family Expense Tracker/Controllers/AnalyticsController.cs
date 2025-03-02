using Family_Expense_Tracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Family_Expense_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly AnalyticsRepository _analyticsRepository;
        public AnalyticsController(AnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        #region AnalyticsByFamilyGroupID
        [HttpGet("GroupWise/{FamilyGroupID}")]
        [Authorize]
        public IActionResult AnalyticsByFamilyGroupID(int FamilyGroupID)
        {
            try
            {
                var analysis = _analyticsRepository.AnalyticsByFamilyGroupID(FamilyGroupID);

                return Ok(analysis);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion


        #region AnalyticsByUserID
        [HttpGet("UserWise/{UserID}")]
        [Authorize]
        public IActionResult AnalyticsByUserID(int UserID)
        {
            try
            {
                var analysis = _analyticsRepository.AnalyticsByUserID(UserID);

                return Ok(analysis);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion
    }
}
