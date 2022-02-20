using ContactBookX.models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookX.storage
{
    class DapperSQLContactStore : IContactStore
    {
        private readonly string _connectionString;
        private readonly int _userId;

        public DapperSQLContactStore(int userId, string connection)
        {
            this._userId = userId;
            this._connectionString = connection;
        }

        public async Task AddContactAsync(ContactInfo contact)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = @$"INSERT INTO Contacts VALUES ( @firstname, @lastname, @id_user )";

            await connection.ExecuteAsync(query, new { firstname = contact.FirstName, lastname = contact.LastName, id_user = _userId });

        }

        public async Task AddEmailAsync(string email, int contactId)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "INSERT INTO emails VALUES (@IdContact, @email  )";

            await connection.ExecuteAsync(query, new { IdContact = contactId, email = email });
        }

        public async Task AddPhoneAsync(string phone, int contactId)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "INSERT INTO phones VALUES (@IdContact, @phone  )";

            await connection.ExecuteAsync(query, new { IdContact = contactId, phone = phone });
        }

        public async Task<IEnumerable<ContactInfo>> GetContactsAsync()
        {
            using var connection = new SqlConnection(_connectionString);

            var contacts = await ReadContactsAsync(connection);
            var phones = await ReadPhonesAsync(connection);
            var emails = await ReadEmailsAsync(connection);

            foreach (var contact in contacts)
            {

                contact.Phones = phones.Where(ph => ph.IdContact == contact.IdContact)
                                       .Select(s => s.phone).ToList();

                contact.Emails = emails.Where(em => em.IdContact == contact.IdContact)
                                       .Select(s => s.email).ToList();
            }

            return contacts;
        }



        private async Task<List<ContactInfo>> ReadContactsAsync(SqlConnection connection)
        {
            string selectContacts = "SELECT * FROM Contacts " +
                                   $"WHERE id_user = {_userId}";

            var contacts = await connection.QueryAsync<ContactInfo>(selectContacts);

            var contactList = contacts.ToList();

            return contactList;
        }

        private async Task<List<Phone>> ReadPhonesAsync(SqlConnection connection)
        {
            string selectPhones = @$"SELECT * FROM phones ph 
                                     INNER JOIN Contacts ct  ON  ct.IdContact = ph.IdContact 
                                     WHERE  ct.id_user = {_userId} ";

            var phones = await connection.QueryAsync<Phone>(selectPhones);
            return phones.ToList();
        }

        private async Task<List<Email>> ReadEmailsAsync(SqlConnection connection)
        {

            string selectEmails = @$"SELECT * FROM emails em 
                                     INNER JOIN Contacts ct  ON  ct.IdContact = em.IdContact 
                                     WHERE  ct.id_user = {_userId} ";

            var emails = await connection.QueryAsync<Email>(selectEmails);
            return emails.ToList();
        }
    }
}
