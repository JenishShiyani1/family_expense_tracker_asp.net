using System.ComponentModel.DataAnnotations;

namespace Family_Expense_Tracker.Models
{
    public class CategoryModel
    {
        public int? CategoryID { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(100, ErrorMessage = "Category Name cannot be more than 100 characters.")]
        public string CategoryName { get; set; }
        public bool IsIncomeCategory { get; set; }
        public int? FamilyGroupID { get; set; }
        public int? UserID { get; set; }
        public string? UserName { get; set; }
    }
    public class CategoryListViewModel
    {
        public IEnumerable<CategoryModel> IncomeCategories { get; set; }
        public IEnumerable<CategoryModel> ExpenseCategories { get; set; }
    }

    public class CategoryDropDown
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

}
