using Family_Expense_Tracker.Models;
using Microsoft.Data.SqlClient;

namespace Family_Expense_Tracker.Data
{
    public class ExpenseRepository
    {
        private readonly IConfiguration _configuration;

        public ExpenseRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region AddExpense
        public bool AddExpense(ExpenseModel expense)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Expenses_Insert", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("FamilyGroupID", expense.FamilyGroupID);
                command.Parameters.AddWithValue("UserID", expense.UserID);
                command.Parameters.AddWithValue("CategoryID", expense.CategoryID);
                command.Parameters.AddWithValue("Amount", expense.Amount);
                command.Parameters.AddWithValue("ExpenseDate", expense.ExpenseDate);
                command.Parameters.AddWithValue("Notes", expense.Notes);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region updateExpense
        public bool UpdateExpense(ExpenseModel expense)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Expenses_Update", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("ExpenseID", expense.ExpenseID);
                command.Parameters.AddWithValue("FamilyGroupID", expense.FamilyGroupID);
                command.Parameters.AddWithValue("UserID", expense.UserID);
                command.Parameters.AddWithValue("CategoryID", expense.CategoryID);
                command.Parameters.AddWithValue("Amount", expense.Amount);
                command.Parameters.AddWithValue("ExpenseDate", expense.ExpenseDate);
                command.Parameters.AddWithValue("Notes", expense.Notes);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region SelectAllExpense
        public List<ExpenseModel> SelectAllExpense(int FamilyGroupID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<ExpenseModel> expense = new List<ExpenseModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Expenses_SelectByFamilyGroup", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("FamilyGroupID", FamilyGroupID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    expense.Add(new ExpenseModel
                    {
                        ExpenseID = Convert.ToInt32(reader["ExpenseID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        ExpenseDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["ExpenseDate"])),
                        Notes = reader["Notes"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    }
                    );
                }
            }
            return expense;
        }
        #endregion

        #region SelectExpenseByID
        public ExpenseModel SelectExpenseByID(int ExpenseID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            ExpenseModel expense = new ExpenseModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Expenses_SelectByPK", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("ExpenseID", ExpenseID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    expense = new ExpenseModel
                    {
                        ExpenseID = Convert.ToInt32(reader["ExpenseID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        ExpenseDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["ExpenseDate"])),
                        Notes = reader["Notes"].ToString(),
                    };
                }
            }
            return expense;
        }
        #endregion
        #region SelectExpenseByUserID
        public List<ExpenseModel> ExpenseByUserID(int UserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<ExpenseModel> expenses = new List<ExpenseModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Expenses_SelectByUser", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("UserID", UserID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    expenses.Add(new ExpenseModel
                    {
                        ExpenseID = Convert.ToInt32(reader["ExpenseID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        ExpenseDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["ExpenseDate"])),
                        Notes = reader["Notes"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    }
                    );
                }
            }
            return expenses;
        }
        #endregion

        #region DeleteExpense
        public bool DeleteExpense(int ExpenseID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Expenses_Delete", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("ExpenseID", ExpenseID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion
    }
}
