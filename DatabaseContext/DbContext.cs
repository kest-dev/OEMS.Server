using MySql.Data.MySqlClient;
using Isopoh.Cryptography.Argon2;
using OEMS.Server.Models;
using System.Security.Principal;

namespace OEMS.Server.DatabaseContext
{
    public class DbContext
    {
        public string ConnectionString { get; set; }

        public DbContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public string IsValidUser(LoginModel model)
        {
            LoginModel user = new LoginModel();

            using (MySqlConnection connection = GetConnection()) 
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user WHERE email = @Email", connection);
                cmd.Parameters.AddWithValue("@Email", "test");

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.Email = reader.GetString(1);
                        user.Password = reader.GetString(2);
                    }
                }

                connection.Close();
            }
            if(Argon2.Verify(user.Password, model.Password))
            {
                return user.Email;
            }

            return "";
        }
    }
}
