using System.ComponentModel.DataAnnotations;

namespace Family_Expense_Tracker.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int FamilyGroupID { get; set; }
        public string GroupName { get; set; }
        public string Role { get; set; }
    }

    public class newUserModel
    {
        public string token { get; set; }
        public UserModel user { get; set; }
    }

    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class AdminRegister
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public int? FamilyGroupID { get; set; }

        [Required(ErrorMessage = "GroupName is required")]  
        public string GroupName { get; set; }
    }

    public class MemberModel
    {
        public int MemberID { get; set; }
        public string FamilyGroupName { get; set; }
        public int MemberUserID { get; set; }
        public string MemberName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime AddedDate { get; set; }
    }

    public class AddMember
    {
        public int? UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? FamilyGroupID { get; set; }
    }
}
