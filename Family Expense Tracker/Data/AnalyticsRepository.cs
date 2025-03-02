using Family_Expense_Tracker.Models;
using Microsoft.Data.SqlClient;

namespace Family_Expense_Tracker.Data
{
    public class AnalyticsRepository
    {
        private readonly IConfiguration _configuration;

        public AnalyticsRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region AnalyticsByFamilyGroupID
        public AnalyticsModel AnalyticsByFamilyGroupID(int FamilyGroupID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            AnalyticsModel analytics = new AnalyticsModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_GetFinancialSummary", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("FamilyGroupID", FamilyGroupID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    analytics = new AnalyticsModel
                    {
                        TotalIncome = Convert.ToDecimal(reader["TotalIncome"]),
                        TotalExpense = Convert.ToDecimal(reader["TotalExpenses"]),
                        TotalSaving = Convert.ToDecimal(reader["TotalSavings"]),
                    };
                }
            }
            return analytics;
        }
        #endregion

        #region AnalyticsByUserID
        public AnalyticsModel AnalyticsByUserID(int UserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            AnalyticsModel analytics = new AnalyticsModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_GetFinancialSummaryForUser", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("UserID", UserID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    analytics = new AnalyticsModel
                    {
                        TotalIncome = Convert.ToDecimal(reader["TotalIncome"]),
                        TotalExpense = Convert.ToDecimal(reader["TotalExpenses"]),
                        TotalSaving = Convert.ToDecimal(reader["TotalSavings"]),
                    };
                }
            }
            return analytics;
        }
        #endregion
    }
}
