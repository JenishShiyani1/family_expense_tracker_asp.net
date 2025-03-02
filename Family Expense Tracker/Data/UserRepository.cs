using Family_Expense_Tracker.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Family_Expense_Tracker.Data
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region AddAdmin
        public bool AddAdmin(AdminRegister user)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Admin_Register", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("Name", user.Name);
                command.Parameters.AddWithValue("Email", user.Email);
                command.Parameters.AddWithValue("Password", user.Password);
                command.Parameters.AddWithValue("GroupName", user.GroupName);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region AddMember
        public bool AddMember(AddMember user)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Member_Add", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("FamilyGroupID", user.FamilyGroupID);
                command.Parameters.AddWithValue("Name", user.Name);
                command.Parameters.AddWithValue("Email", user.Email);
                command.Parameters.AddWithValue("Password", user.Password);

                connection.Open();

                int rowAffected = command.ExecuteNonQuery();

                return rowAffected > 0;
            }
        }
        #endregion

        #region Admin&MemberLogin
        public object AdminMemberLogin(UserLoginModel user)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            UserModel user1 = new UserModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_User_Login", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("Email", user.Email);
                command.Parameters.AddWithValue("Password", user.Password);
                command.Parameters.AddWithValue("Role", user.Role);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    user1 = new UserModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString(),
                        FamilyGroupID = Convert.ToInt32(reader["FamilyGroupID"]),
                        GroupName = reader["GroupName"].ToString(),
                    };
                }
            }
            if(user1.Email == null)
            {
                throw new Exception("Invalid Email, Password or Role");
            }
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: new[]
                {
            new Claim(ClaimTypes.Name, user1.Name),
            new Claim(ClaimTypes.Email, user1.Email),
            new Claim(ClaimTypes.Role, user1.Role),
            new Claim("UserID", user1.UserID.ToString()),
            new Claim("FamilyGroupID", user1.FamilyGroupID.ToString())
                },
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // ✅ Return token with user details
            return new
            {
                Token = tokenString,
                User = user1
            };
        }
        #endregion


        #region FamilyGroupWise Member
        public List<MemberModel> GroupWiseMember(int AdminID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            var members = new List<MemberModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Admin_Dashboard", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("AdminUserID", AdminID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    members.Add(new MemberModel
                    {
                        MemberID = Convert.ToInt32(reader["MemberID"]),
                        FamilyGroupName = reader["FamilyGroupName"].ToString(),
                        MemberUserID = Convert.ToInt32(reader["MemberUserID"]),
                        MemberName = reader["MemberName"].ToString(),
                        Email = reader["EmailID"].ToString(),
                        Role = reader["MemberRole"].ToString(),
                    });
                }
            }
            return members;
        }
        #endregion
        #region Member&AdminSelect
        public MemberAdminModel MemberAdminSelect(int memberUserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            var dashboardModel = new MemberAdminModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Member_Dashboard", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("MemberUserID", memberUserID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    dashboardModel.FamilyGroupName = reader["FamilyGroupName"].ToString();
                    dashboardModel.AdminName = reader["AdminName"].ToString();
                }

                if (reader.NextResult())
                {
                    dashboardModel.Members = new List<MemberModel>();
                    while (reader.Read())
                    {
                        dashboardModel.Members.Add(new MemberModel
                        {
                            MemberUserID = Convert.ToInt32(reader["MemberUserID"]),
                            MemberID = Convert.ToInt32(reader["MemberID"]),
                            MemberName = reader["MemberName"].ToString(),
                            Role = reader["MemberRole"].ToString(),
                        });
                    }
                }
            }

            if(dashboardModel.AdminName == null)
            {
                throw new Exception("Enter valid memberID to get admin details");
            }

            return dashboardModel;
        }
        #endregion
        #region UpdateUserDetails
        public bool UpdateUserDetails(AddMember user)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Update_User_Details", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("UserID", user.UserID);
                command.Parameters.AddWithValue("Name", user.Name);
                command.Parameters.AddWithValue("Email", user.Email);
                command.Parameters.AddWithValue("Password", user.Password);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion

        #region UpdateFamilyGroupName
        public bool UpdateFamilyGroupName(int familyGroupID, string groupName)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Update_FamilyGroupName", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("FamilyGroupID", familyGroupID);
                command.Parameters.AddWithValue("GroupName", groupName);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion

        #region RemoveMember
        public bool RemoveMember(int memberUserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Remove_Member", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };

                command.Parameters.AddWithValue("MemberUserID", memberUserID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        #endregion


        #region SelectUserByID

        public AddMember SelectUserByID(int UserID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            AddMember member = new AddMember();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("PR_Select_MemberByID", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                command.Parameters.AddWithValue("UserID", UserID);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    member = new AddMember
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                    };
                }
            }
            return member;
        }
        #endregion

    }
}
