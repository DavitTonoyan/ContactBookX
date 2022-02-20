using ContactBookX.models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace ContactBookX.storage
{
    class AdoNetSQLContactStore : IContactStore
    {
        private readonly int _userId;
        private readonly string _connectionString;

        public AdoNetSQLContactStore(int userId, string connection)
        {
            this._userId = userId;
            this._connectionString = connection;
        }

        public async Task AddContactAsync(ContactInfo contact)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @$"INSERT INTO Contacts VALUES ( @firstname, @lastname, {_userId} )";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("firstname", contact.FirstName);
            command.Parameters.AddWithValue("lastname", contact.LastName);

            connection.Open();

            await command.ExecuteNonQueryAsync();
        }

        public async Task AddEmailAsync(string email, int contactId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = "INSERT INTO emails VALUES" +
                           $"( {contactId} , @email )";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("email", email);


            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task AddPhoneAsync(string phone, int contactId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = "INSERT INTO phones VALUES" +
                           $"( {contactId} , @phone )";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("phone", phone);

            connection.Open();
            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<ContactInfo>> GetContactsAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var contacts = await ReadContactsAsync(connection);
            var phones = await ReadPhonesAsync(connection);
            var emails = await ReadEmailsAsync(connection);

            foreach (var contact in contacts)
            {
                contact.Phones = phones.Where(ph => ph.IdContact == contact.IdContact).Select(s => s.phone).ToList();
                contact.Emails = emails.Where(em => em.IdContact == contact.IdContact).Select(s => s.email).ToList();
            }

            return contacts;
        }



        private async Task<List<ContactInfo>> ReadContactsAsync(SqlConnection connection)
        {
            string selectContacts = "SELECT * FROM Contacts " +
                                   $"WHERE id_user = {_userId}";

            using var commandContacts = new SqlCommand(selectContacts, connection);
            using var reader = await commandContacts.ExecuteReaderAsync();

            List<ContactInfo> contacts = new();

            while (await reader.ReadAsync())
            {
                int idOrdinal = reader.GetOrdinal("IdContact");
                int id = reader.GetInt32(idOrdinal);

                int firstnameOrdinal = reader.GetOrdinal("firstname");
                string firstname = reader.GetString(firstnameOrdinal);

                int lastnameOrdinal = reader.GetOrdinal("lastname");
                string lastname = reader.GetString(lastnameOrdinal);

                ContactInfo contact = new ContactInfo()
                {
                    IdContact = id,
                    FirstName = firstname,
                    LastName = lastname
                };

                contacts.Add(contact);
            }

            return contacts;
        }

        private async Task<List<Phone>> ReadPhonesAsync(SqlConnection connection)
        {
            List<Phone> phones = new();

            string selectPhones = @$"SELECT * FROM phones ph 
                                     INNER JOIN Contacts ct  ON  ct.IdContact = ph.IdContact 
                                     WHERE  ct.id_user = {_userId} ";

            using var commandPhones = new SqlCommand(selectPhones, connection);
            using var readerPhones = await commandPhones.ExecuteReaderAsync();

            while (await readerPhones.ReadAsync())
            {
                int idOrdinal = readerPhones.GetOrdinal("IdContact");
                int id = readerPhones.GetInt32(idOrdinal);

                int phoneOrdinal = readerPhones.GetOrdinal("phone");
                string phone = readerPhones.GetString(phoneOrdinal);

                Phone ph = new Phone()
                {
                    IdContact = id,
                    phone = phone
                };
                phones.Add(ph);
            }

            return phones;
        }

        private async Task<List<Email>> ReadEmailsAsync(SqlConnection connection)
        {
            List<Email> emails = new();

            string selectEmails = @$"SELECT * FROM emails em 
                                     INNER JOIN Contacts ct  ON  ct.IdContact = em.IdContact 
                                     WHERE  ct.id_user = {_userId} ";


            using var commandEmails = new SqlCommand(selectEmails, connection);
            using var readerEmails = await commandEmails.ExecuteReaderAsync();

            while (await readerEmails.ReadAsync())
            {
                int idOrdinal = readerEmails.GetOrdinal("IdContact");
                int id = readerEmails.GetInt32(idOrdinal);

                int emailOrdinal = readerEmails.GetOrdinal("email");
                string email = readerEmails.GetString(emailOrdinal);

                Email em = new Email()
                {
                    IdContact = id,
                    email = email
                };

                emails.Add(em);
            }
            return emails;
        }
    }
}


