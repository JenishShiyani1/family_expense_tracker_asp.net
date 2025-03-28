﻿using System.ComponentModel.DataAnnotations;

namespace Family_Expense_Tracker.Models
{
    public class IncomeModel
    {
        public int? IncomeID { get; set; }
        public int? FamilyGroupID { get; set; }
        public int? UserID { get; set; }
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Income date is required.")]
        [DataType(DataType.Date)]
        public DateOnly IncomeDate { get; set; }

        [Required(ErrorMessage = "Add some Notes")]
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
