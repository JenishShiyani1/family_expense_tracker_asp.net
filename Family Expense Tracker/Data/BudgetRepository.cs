using Family_Expense_Tracker.Models;
using Microsoft.Data.SqlClient;

namespace Family_Expense_Tracker.Data
{
    public class BudgetRepository
    {

        private readonly IConfiguration _configuration;

        public BudgetRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region AddBudget
        public bool AddBudget(BudgetModel budget)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Budget_Insert", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("FamilyGroupID", budget.FamilyGroupID);
                command.Parameters.AddWithValue("UserID", budget.UserID);
                command.Parameters.AddWithValue("CategoryID", budget.CategoryID);
                command.Parameters.AddWithValue("TotalBudget", budget.TotalBudget);
                command.Parameters.AddWithValue("EndDate", budget.EndDate);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region updateBudget
        public bool UpdateBudget(BudgetModel budget)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Budget_Update", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("BudgetID", budget.BudgetID);
                command.Parameters.AddWithValue("FamilyGroupID", budget.FamilyGroupID);
                command.Parameters.AddWithValue("UserID", budget.UserID);
                command.Parameters.AddWithValue("CategoryID", budget.CategoryID);
                command.Parameters.AddWithValue("TotalBudget", budget.TotalBudget);
                command.Parameters.AddWithValue("EndDate", budget.EndDate);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region SelectAllBudget
        public List<BudgetModel> SelectAllBudget(int FamilyGroupID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<BudgetModel> budget = new List<BudgetModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Budget_SelectByFamilyGroup", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("FamilyGroupID", FamilyGroupID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    budget.Add(new BudgetModel
                    {
                        BudgetID = Convert.ToInt32(reader["BudgetID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        TotalBudget = Convert.ToDecimal(reader["TotalBudget"]),
                        RemainingBudget = Convert.ToDecimal(reader["RemainingBudget"]),
                        EndDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["EndDate"])),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    }
                    );
                }
            }
            return budget;
        }
        #endregion

        #region SelectBudgetByID
        public BudgetModel SelectBudgetByID(int BudgetID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            BudgetModel budget = new BudgetModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Budget_SelectByPK", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("BudgetID", BudgetID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    budget = new BudgetModel
                    {
                        BudgetID = Convert.ToInt32(reader["BudgetID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        TotalBudget = Convert.ToDecimal(reader["TotalBudget"]),
                        RemainingBudget = Convert.ToDecimal(reader["RemainingBudget"]),
                        EndDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["EndDate"])),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    };
                }
            }
            return budget;
        }
        #endregion

        #region SelectBudgetByUserID
        public List<BudgetModel> BudgetByUserID(int UserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<BudgetModel> budgets = new List<BudgetModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Budget_SelectByUserID", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("UserID", UserID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    budgets.Add(new BudgetModel
                    {
                        BudgetID = Convert.ToInt32(reader["BudgetID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        TotalBudget = Convert.ToDecimal(reader["TotalBudget"]),
                        RemainingBudget = Convert.ToDecimal(reader["RemainingBudget"]),
                        EndDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["EndDate"])),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    }
                    );
                }
            }
            return budgets;
        }
        #endregion

        #region DeleteBudget
        public bool DeleteBudget(int BudgetID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Budget_Delete", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("BudgetID", BudgetID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion

        #region GetRemainingBudget
        public RemainingBudgetModel GetRemainingBudget(int FamilyGroupID , int UserID , int CategoryID , string ExpenseDate)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            RemainingBudgetModel remainingRudget = new RemainingBudgetModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Budget_GetRemaining", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("FamilyGroupID", FamilyGroupID);
                command.Parameters.AddWithValue("UserID", UserID);
                command.Parameters.AddWithValue("CategoryID", CategoryID);
                command.Parameters.AddWithValue("ExpenseDate", ExpenseDate);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    remainingRudget = new RemainingBudgetModel
                    {
                        RemainingBudget = Convert.ToDecimal(reader["RemainingBudget"]),
                        EndDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["EndDate"]))
                    };
                }
            }
            return remainingRudget;
        }
        #endregion
    }
}
