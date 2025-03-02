using Family_Expense_Tracker.Models;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

namespace Family_Expense_Tracker.Data
{
    public class IncomeRepository
    {

        private readonly IConfiguration _configuration;

        public IncomeRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region AddIncome
        public bool AddIncome(IncomeModel income)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Income_Insert", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("FamilyGroupID", income.FamilyGroupID);
                command.Parameters.AddWithValue("UserID", income.UserID);
                command.Parameters.AddWithValue("CategoryID", income.CategoryID);
                command.Parameters.AddWithValue("Amount", income.Amount);
                command.Parameters.AddWithValue("IncomeDate", income.IncomeDate);
                command.Parameters.AddWithValue("Notes", income.Notes);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region updateIncome
        public bool UpdateIncome(IncomeModel income)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Income_Update", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("IncomeID", income.IncomeID);
                command.Parameters.AddWithValue("FamilyGroupID", income.FamilyGroupID);
                command.Parameters.AddWithValue("UserID", income.UserID);
                command.Parameters.AddWithValue("CategoryID", income.CategoryID);
                command.Parameters.AddWithValue("Amount", income.Amount);
                command.Parameters.AddWithValue("IncomeDate", income.IncomeDate);
                command.Parameters.AddWithValue("Notes", income.Notes);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region SelectAllIncome
        public List<IncomeModel> SelectAllIncome(int FamilyGroupID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<IncomeModel> income = new List<IncomeModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Income_SelectByFamilyGroupID", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("FamilyGroupID", FamilyGroupID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    income.Add(new IncomeModel
                    {
                        IncomeID = Convert.ToInt32(reader["IncomeID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        IncomeDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["IncomeDate"])),
                        Notes = reader["Notes"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    }
                    );
                }
            }
            return income;
        }
        #endregion

        #region SelectIncomeByID
        public IncomeModel SelectIncomeByID(int IncomeID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            IncomeModel income = new IncomeModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Income_SelectByPK", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("IncomeID", IncomeID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    income = new IncomeModel
                    {
                        IncomeID = Convert.ToInt32(reader["IncomeID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        IncomeDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["IncomeDate"])),
                        Notes = reader["Notes"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    };
                }
            }
            return income;
        }
        #endregion
        #region SelectIncomeByUserID
        public List<IncomeModel> IncomeByUserID(int UserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<IncomeModel> incomes = new List<IncomeModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Income_SelectByUserID", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("UserID", UserID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    incomes.Add(new IncomeModel
                    {
                        IncomeID = Convert.ToInt32(reader["IncomeID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        IncomeDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["IncomeDate"])),
                        Notes = reader["Notes"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    }
                    );
                }
            }
            return incomes;
        }
        #endregion

        #region DeleteIncome
        public bool DeleteIncome(int IncomeID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Income_Delete", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("IncomeID", IncomeID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion
    }
}
