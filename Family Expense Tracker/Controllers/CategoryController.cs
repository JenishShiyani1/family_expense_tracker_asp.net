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
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        #region AddCategory
        [HttpPost]
        [Authorize]
        public IActionResult AddCategory(CategoryModel category)
        {
            if(category == null)
            {
                return BadRequest();
            }
            try
            {
                bool isInserted = _categoryRepository.AddCategory(category);

                if (isInserted)
                {
                    return Ok(new { Status = "Success", Message = "Category Inserted Successfully" });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return StatusCode(500, "An error occurred while inserting Category");
        }
        #endregion

        #region UpdateCategory
        [HttpPut("{ID}")]
        [Authorize]
        public IActionResult UpdateCategory(int ID , CategoryModel category)
        {
            if(ID != category.CategoryID || category == null || category.CategoryID <= 0)
            {
                return BadRequest(new { message = "Invalid category details provided" });
            }

            try
            {
                bool isUpdated = _categoryRepository.UpdateCategory(category);

                if (isUpdated)
                {
                    return Ok(new { Status = "Success", Message = "Category details updated successfully" });
                }
                else
                {
                    return NotFound(new { message = "Category not found or no changes made" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        #endregion

        #region SelectAllCategory
        [HttpGet("GroupWise{FamilyGroupID}")]
        [Authorize]
        public IActionResult SelectAllCategory(int FamilyGroupID)
        {
            try
            {
                var categories = _categoryRepository.SelectAllCategory(FamilyGroupID);

                if (categories.Count != 0)
                {

                    return Ok(categories);
                }
                else
                {

                    return NotFound(new { Message = "There is no Category in family group" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region SelectCategoryByID
        [HttpGet("{CategoryID}")]
        [Authorize]
        public IActionResult SelectCategoryByID(int CategoryID)
        {
            try
            {
                var categories = _categoryRepository.SelectCategoryByID(CategoryID);

                    return Ok(categories);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region IncomeCategoryByUser
        [HttpGet("Income/{FamilyGroupID}")]
        [Authorize]
        public IActionResult IncomeCategoryByUser(int FamilyGroupID)
        {
            try
            {
                var categories = _categoryRepository.IncomeCategoryByUser(FamilyGroupID);
                if (categories.Count != 0)
                {
                    return Ok(categories);
                }
                else
                {
                    return NotFound(new { Message = "There is no Income Category for this user" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region ExpenseCategoryByUser
        [HttpGet("Expense/{FamilyGroupID}")]
        [Authorize]
        public IActionResult ExpenseCategoryByUser(int FamilyGroupID)
        {
            try
            {
                var categories = _categoryRepository.ExpenseCategoryByUser(FamilyGroupID);
                if (categories.Count != 0)
                {
                    return Ok(categories);
                }
                else
                {
                    return NotFound(new { Message = "There is no Expense Category for this user" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        #endregion

        #region DeleteCategory
        [HttpDelete("{CategoryID}")]
        [Authorize]
        public IActionResult DeleteCategory(int CategoryID)
        {
            if (CategoryID <= 0)
            {
                return BadRequest(new { message = "Invalid Category ID provided" });
            }

            try
            {
                bool isRemoved = _categoryRepository.DeleteCategory(CategoryID);

                if (isRemoved)
                {
                    return Ok(new { Status = "Success", Message = "Category removed successfully" });
                }
                else
                {
                    return NotFound(new { message = "Category not found or already removed" });
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
