using Family_Expense_Tracker.Models;
using Microsoft.Data.SqlClient;

namespace Family_Expense_Tracker.Data
{
    public class SavingRepository
    {
        private readonly IConfiguration _configuration;

        public SavingRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region AddSaving
        public bool AddSaving(SavingModel saving)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Savings_Insert", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("FamilyGroupID", saving.FamilyGroupID);
                command.Parameters.AddWithValue("UserID", saving.UserID);
                command.Parameters.AddWithValue("SavingName", saving.SavingName);
                command.Parameters.AddWithValue("TargetAmount", saving.TargetedAmount);
                command.Parameters.AddWithValue("Deadline", saving.DeadLine);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region updateSaving
        public bool UpdateSaving(SavingModel saving)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Savings_Update", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("SavingID", saving.SavingID);
                command.Parameters.AddWithValue("FamilyGroupID", saving.FamilyGroupID);
                command.Parameters.AddWithValue("UserID", saving.UserID);
                command.Parameters.AddWithValue("SavingName", saving.SavingName);
                command.Parameters.AddWithValue("TargetAmount", saving.TargetedAmount);
                command.Parameters.AddWithValue("Deadline", saving.DeadLine);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region SelectAllSaving
        public List<SavingModel> SelectAllSaving(int FamilyGroupID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<SavingModel> saving = new List<SavingModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Savings_SelectByFamilyGroup", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("FamilyGroupID", FamilyGroupID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    saving.Add(new SavingModel
                    {
                        SavingID = Convert.ToInt32(reader["SavingID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        SavingName = reader["SavingName"].ToString(),
                        TargetedAmount = Convert.ToDecimal(reader["TargetAmount"]),
                        SavedAmount = Convert.ToDecimal(reader["SavedAmount"]),
                        DeadLine = DateOnly.FromDateTime(Convert.ToDateTime(reader["DeadLine"])),
                        Status = reader["Status"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    }
                    );
                }
            }
            return saving;
        }
        #endregion

        #region SelectSavingByID
        public SavingModel SelectSavingByID(int SavingID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SavingModel saving = new SavingModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Savings_SelectByPK", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("SavingID", SavingID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    saving = new SavingModel
                    {
                        SavingID = Convert.ToInt32(reader["SavingID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        SavingName = reader["SavingName"].ToString(),
                        TargetedAmount = Convert.ToDecimal(reader["TargetAmount"]),
                        SavedAmount = Convert.ToDecimal(reader["SavedAmount"]),
                        DeadLine = DateOnly.FromDateTime(Convert.ToDateTime(reader["DeadLine"])),
                        Status = reader["Status"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    };
                }
            }
            return saving;
        }
        #endregion

        #region SelectSavingByUserID
        public List<SavingModel> SavingByUserID(int UserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<SavingModel> savings = new List<SavingModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Savings_SelectByUserID", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("UserID", UserID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    savings.Add(new SavingModel
                    {
                        SavingID = Convert.ToInt32(reader["SavingID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        SavingName = reader["SavingName"].ToString(),
                        TargetedAmount = Convert.ToDecimal(reader["TargetAmount"]),
                        SavedAmount = Convert.ToDecimal(reader["SavedAmount"]),
                        DeadLine = DateOnly.FromDateTime(Convert.ToDateTime(reader["DeadLine"])),
                        Status = reader["Status"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    }
                    );
                }
            }
            return savings;
        }
        #endregion

        #region DeleteSavings
        public bool DeleteSavings(int SavingID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Savings_Delete", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("SavingID", SavingID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion

        #region updateSavingAmount
        public bool UpdateSavingAmount(int ID , decimal SavedAmount , int UserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Savings_UpdateSavedAmount", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("SavingID", ID);
                command.Parameters.AddWithValue("Amount", SavedAmount);
                command.Parameters.AddWithValue("MemberUserID", UserID);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region SelectSavingContribution
        public List<SavingContribution> SelectSavingContribution(int UserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<SavingContribution> savings = new List<SavingContribution>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_GetUserContributions", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("UserID", UserID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    savings.Add(new SavingContribution
                    {
                        SavingID = Convert.ToInt32(reader["SavingID"]),
                        SavingName = reader["SavingName"].ToString(),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["Name"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        AddDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["ContributedAt"])),
                    });
                }
            }
            return savings;
        }
        #endregion
    }
}
