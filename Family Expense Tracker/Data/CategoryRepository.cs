using Family_Expense_Tracker.Models;
using Microsoft.Data.SqlClient;

namespace Family_Expense_Tracker.Data
{
    public class CategoryRepository
    {
        private readonly IConfiguration _configuration;

        public CategoryRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region AddCategory
        public bool AddCategory(CategoryModel category)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Categories_Insert", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("CategoryName", category.CategoryName);
                command.Parameters.AddWithValue("IsIncomeCategory", category.IsIncomeCategory);
                command.Parameters.AddWithValue("FamilyGroupID", category.FamilyGroupID);
                command.Parameters.AddWithValue("UserID", category.UserID);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region updateCategory
        public bool UpdateCategory(CategoryModel category)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Categories_Update", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("CategoryID", category.CategoryID);
                command.Parameters.AddWithValue("CategoryName", category.CategoryName);
                command.Parameters.AddWithValue("IsIncomeCategory", category.IsIncomeCategory);
                command.Parameters.AddWithValue("FamilyGroupID", category.FamilyGroupID);
                command.Parameters.AddWithValue("UserID", category.UserID);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region SelectAllCategory
        public List<CategoryModel> SelectAllCategory(int FamilyGroupID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<CategoryModel> category = new List<CategoryModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Categories_SelectAll", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("FamilyGroupID", FamilyGroupID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    category.Add(new CategoryModel
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        IsIncomeCategory = Convert.ToBoolean(reader["IsIncomeCategory"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["Name"].ToString(),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                    }
                    );
                }
            }
            return category;
        }
        #endregion

        #region SelectCategoryByID
        public CategoryModel SelectCategoryByID(int CategoryID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            CategoryModel category = new CategoryModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Categories_SelectByID", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("CategoryID", CategoryID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    category = new CategoryModel
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                        IsIncomeCategory = Convert.ToBoolean(reader["IsIncomeCategory"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                    };
                }
            }
            return category;
        }
        #endregion
        #region IncomeCategoryByUser
        public List<CategoryDropDown> IncomeCategoryByUser(int FamilyGroupID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<CategoryDropDown> category = new List<CategoryDropDown>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Categories_SelectByUser", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("FamilyGroupID", FamilyGroupID);
                command.Parameters.AddWithValue("IsIncomeCategory", true);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    category.Add(new CategoryDropDown
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                    }
                    );
                }
            }
            return category;
        }
        #endregion

        #region ExpenseCategoryByUser
        public List<CategoryDropDown> ExpenseCategoryByUser(int FamilyGroupID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            List<CategoryDropDown> category = new List<CategoryDropDown>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Categories_SelectByUser", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("FamilyGroupID", FamilyGroupID);
                command.Parameters.AddWithValue("IsIncomeCategory", false);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    category.Add(new CategoryDropDown
                    {
                        CategoryID = Convert.ToInt32(reader["CategoryID"]),
                        CategoryName = reader["CategoryName"].ToString(),
                    }
                    );
                }
            }
            return category;
        }
        #endregion

        #region DeleteCategory
        public bool DeleteCategory(int CategoryID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Categories_Delete", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("CategoryID", CategoryID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion
    }
}
