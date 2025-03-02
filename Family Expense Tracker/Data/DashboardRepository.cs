using Family_Expense_Tracker.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;

namespace Family_Expense_Tracker.Data
{
    public class DashboardRepository
    {

        private readonly IConfiguration _configuration;

        public DashboardRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region MemberSideDashboard

        public FinanceDashboardData GetFinanceDashboardData(int UserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            FinanceDashboardData data = new FinanceDashboardData
            {
                Counts = new List<DashboardCountsModel>(),
                CategoryWiseExpenses = new List<CategoryWiseExpense>(),
                FamilyMembers = new List<FamilyMember>(),
                FamilyAdmins = new List<FamilyAdmin>(),
                RecentExpenses = new List<RecentExpense>(),
                RecentIncomes = new List<RecentIncome>(),
            };

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetFinanceDashboardData", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("UserID", UserID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        data.Counts.Add(new DashboardCountsModel
                        {
                            Metric = reader["Metric"].ToString(),
                            Value = Convert.ToDecimal(reader["Value"])

                        });
                    }
                }
                if (reader.NextResult() && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.CategoryWiseExpenses.Add(new CategoryWiseExpense
                        {
                            CategoryName = reader["CategoryName"].ToString(),
                            TotalExpense = Convert.ToDecimal(reader["TotalExpense"])
                        });
                    }
                }
                if (reader.NextResult() && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.FamilyMembers.Add(new FamilyMember
                        {
                            MemberName = reader["MemberName"].ToString(),
                            Role = reader["Role"].ToString()
                        });
                    }
                }
                if (reader.NextResult() && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.FamilyAdmins.Add(new FamilyAdmin
                        {
                            FamilyGroupName = reader["FamilyGroupName"].ToString(),
                            AdminName = reader["AdminName"].ToString()
                        });
                    }
                }
                if (reader.NextResult() && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.RecentExpenses.Add(new RecentExpense
                        {
                            ExpenseID = Convert.ToInt32(reader["ExpenseID"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"]),
                            Notes = reader["Notes"].ToString()
                        });
                    }
                }
                if (reader.NextResult() && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.RecentIncomes.Add(new RecentIncome
                        {
                            IncomeID = Convert.ToInt32(reader["IncomeID"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            IncomeDate = Convert.ToDateTime(reader["IncomeDate"]),
                            Notes = reader["Notes"].ToString()
                        });
                    }
                }
            }
            return data;
        }

        #endregion


        #region AdminSideDashboard
        public AdminFinanceDashboardData GetAdminFinanceDashboardData(int UserID, int FamilyGroupID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            AdminFinanceDashboardData data = new AdminFinanceDashboardData
            {
                Counts = new List<AdminDashboardCountsModel>(),
                CategoryWiseExpenses = new List<AdminCategoryWiseExpense>(),
                FamilyGroupName = new FamilyGroupName(),
                AdminSideMember = new List<AdminSideMember>(),
                RecentExpenses = new List<AdminRecentExpense>(),
                RecentIncomes = new List<AdminRecentIncome>(),
            };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAdminFinanceDashboardData", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("UserID", UserID);
                command.Parameters.AddWithValue("FamilyGroupID", FamilyGroupID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.Counts.Add(new AdminDashboardCountsModel
                        {
                            Metric = reader["Metric"].ToString(),
                            Value = Convert.ToDecimal(reader["Value"])

                        });
                    }
                }
                if (reader.NextResult() && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.CategoryWiseExpenses.Add(new AdminCategoryWiseExpense
                        {
                            CategoryName = reader["CategoryName"].ToString(),
                            TotalExpense = Convert.ToDecimal(reader["TotalExpense"])
                        });
                    }
                }
                if (reader.NextResult() && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.FamilyGroupName = new FamilyGroupName
                        {
                            FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                            GroupName = reader["GroupName"].ToString()
                        };
                    }
                }
                if (reader.NextResult() && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.AdminSideMember.Add(new AdminSideMember
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            GroupName = reader["GroupName"].ToString(),
                        });
                    }
                }
                if (reader.NextResult() && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.RecentExpenses.Add(new AdminRecentExpense
                        {
                            ExpenseID = Convert.ToInt32(reader["ExpenseID"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"]),
                            Notes = reader["Notes"].ToString()
                        });
                    }
                }
                if (reader.NextResult() && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.RecentIncomes.Add(new AdminRecentIncome
                        {
                            IncomeID = Convert.ToInt32(reader["IncomeID"]),
                            CategoryName = reader["CategoryName"].ToString(),
                            Amount = Convert.ToDecimal(reader["Amount"]),
                            IncomeDate = Convert.ToDateTime(reader["IncomeDate"]),
                            Notes = reader["Notes"].ToString()
                        });
                    }
                }
            }
            return data;
        }

        #endregion
    }
}
