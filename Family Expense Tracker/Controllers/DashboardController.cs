using Family_Expense_Tracker.BAL;
using Family_Expense_Tracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Family_Expense_Tracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class DashboardController : ControllerBase
    {
        private readonly DashboardRepository _dashboardRepository;
        public DashboardController(DashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        #region MemberDashboard

        [HttpGet("Member")]
        [Authorize]

        public IActionResult GetMemberDashboardData(int UserID)
        {
            try
            {
                var dashboard = _dashboardRepository.GetFinanceDashboardData(UserID);

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region AdminDashboard

        [HttpGet("Admin")]
        [Authorize]

        public IActionResult GetAdminDashboardData(int UserID, int FamilyGroupID)
        {
            try
            {
                var dashboard = _dashboardRepository.GetAdminFinanceDashboardData(UserID, FamilyGroupID);

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}
