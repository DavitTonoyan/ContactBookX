using ContactBookX.models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ContactBookX.storage
{
    class AdoNetSQLUserStore : IUserStore
    {
        public string ConntectionString { get; }

        public AdoNetSQLUserStore(string connection)
        {
            ConntectionString = connection;
        }

        public async Task<IEnumerable<UserInfo>> GetUsersAsync()
        {
            using var connection = new SqlConnection(ConntectionString);

            List<UserInfo> usersList = new();

            using var command = new SqlCommand("select * from Users", connection);
            connection.Open();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var idUserOrdinal = reader.GetOrdinal("id_user");
                int idUser = reader.GetInt32(idUserOrdinal);

                var usernameOrdinal = reader.GetOrdinal("username");
                string username = reader.GetString(usernameOrdinal);

                var passwordOrdinal = reader.GetOrdinal("password");
                string password = reader.GetString(passwordOrdinal);

                var firstnameOrdinal = reader.GetOrdinal("firstname");
                string firstname = reader.GetString(firstnameOrdinal);

                var lastnameOrdinal = reader.GetOrdinal("lastname");
                string lastname = reader.GetString(lastnameOrdinal);


                UserInfo user = new UserInfo()
                {
                    IdUser = idUser,
                    UserName = username,
                    Password = password,
                    FirstName = firstname,
                    LastName = lastname
                };

                usersList.Add(user);
            }

            return usersList;
        }
        
        public async Task AddUserAsync(UserInfo user)
        {
            using var connection = new SqlConnection(ConntectionString);
            string insertUser = "Insert into Users Values" +
                                "( @username, @password, @firstname, @lastname )";


            using var command = new SqlCommand(insertUser, connection);

            command.Parameters.AddWithValue("username", user.UserName);
            command.Parameters.AddWithValue("password", user.Password);
            command.Parameters.AddWithValue("firstname", user.FirstName);
            command.Parameters.AddWithValue("lastname", user.LastName);

            connection.Open();

            await command.ExecuteNonQueryAsync();
        }
    }

}
