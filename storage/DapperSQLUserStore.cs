using ContactBookX.models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ContactBookX.storage
{
    class DapperSQLUserStore : IUserStore
    {
        public string ConntectionString { get; }

        public DapperSQLUserStore(string connect)
        {
            ConntectionString = connect;
        }

        public async Task AddUserAsync(UserInfo user)
        {
            using var connection = new SqlConnection(ConntectionString);
            connection.Open();

            string insertUser = @"INSERT INTO Users VALUES ( @username, @password, @firstname, @lastname )";

            await connection.ExecuteAsync(insertUser, user);
        }

        public async Task<IEnumerable<UserInfo>> GetUsersAsync()
        {
            using var connection = new SqlConnection(ConntectionString);
            string selectUsers = @"Select id_user IdUser, username UserName, password Password, firstname FirstName, lastname LastName 
                                   FROM Users";

            return await connection.QueryAsync<UserInfo>(selectUsers);
        }
    }
}
