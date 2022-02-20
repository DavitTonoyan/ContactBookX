using ContactBookX.BussinesLogic;
using ContactBookX.models;
using ContactBookX.storage;
using ContactBookX.Tables;
using System;
using System.Threading.Tasks;


using ContactBookX.Logging;
namespace ContactBookX.UI
{
    class WorkUI
    {
        private readonly ILogger _logger;
        private readonly NotepadManager _logic;

        public WorkUI(IDbConnection connection, ILogger logger)
        {
            string conntectionString = connection.GetConnectionString();
            var dapperUserStore = new AdoNetSQLUserStore(conntectionString);
            _logic = new NotepadManager(dapperUserStore);

            _logger = logger;
        }

        public async Task Start()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Add User \n2.Sign in to user   \n" +
                                  " Press Esc to finish program ");

                ConsoleKey key = Console.ReadKey().Key;
                Console.WriteLine("\n\n");

                switch (key)
                {
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:

                        await AddUser();
                        break;

                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:

                        await SignInToAccount();
                        break;

                    case ConsoleKey.Escape:
                        return;
                }

            }
        }

        private async Task AddUser()
        {
            string username = ReadWithLabel("Enter username:  ");
            bool isUsernameExist = await _logic.ValidateUsername(username);

            if (isUsernameExist)
            {
                string warning = " The username is already exist ";
                await _logger.Warning(warning);

                Console.WriteLine(warning);
                Console.ReadKey();
                return;
            }

            string password = ReadWithLabel("Enter Password:  ");
            string firstname = ReadWithLabel("Enter first name:  ");
            string lastname = ReadWithLabel("Enter last name:  ");

            await _logic.AddUser(username, password, firstname, lastname);
            await _logger.Information("New user created ");
        }
        private async Task SignInToAccount()
        {
            string username = ReadWithLabel("Enter username:  ");
            string password = ReadWithLabel("Enter password:  ");
            try
            {
                var contacts = await _logic.SignIn(username, password);
                await _logger.Information($" Username: {username}  signed in to account ");

                await EditUser(contacts);
                await _logger.Information($" Username: {username} signed out from account ");
            }
            catch (InvalidOperationException ex)
            {
                await _logger.Error(ex.Message);
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private async Task EditUser(IContactStore contacts)
        {
            while (true)
            {
                Console.Clear();
                await ShowContacts(contacts);
                Console.WriteLine("\n\n");

                Console.WriteLine("1. Add Contact  \n2. Add Phone  \n3. Add Email \n" +
                                  "Press Esc to Sign Out ");

                ConsoleKey key = Console.ReadKey().Key;
                Console.WriteLine("\n\n");

                switch (key)
                {

                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:

                        await AddContact(contacts);
                        break;

                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:

                        await AddPhoneToContact(contacts);
                        break;

                    case ConsoleKey.NumPad3:
                    case ConsoleKey.D3:

                        await AddEmailToContact(contacts);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }


        private async Task ShowContacts(IContactStore contacts)
        {
            string str = await _logic.ShowContacts(contacts);
            Console.WriteLine(str);
        }

        private async Task AddContact(IContactStore contacts)
        {
            string firstname = ReadWithLabel("Enter first name:  ");
            string lastname = ReadWithLabel("Enter last name:  ");


            ContactInfo contact = new ContactInfo()
            {
                FirstName = firstname,
                LastName = lastname
            };
            await contacts.AddContactAsync(contact);
        }

        private async Task AddPhoneToContact(IContactStore contacts)
        {
            int idContact = int.Parse(ReadWithLabel("Enter Id of contact: "));
            string phone = ReadWithLabel("Enter phone number:  ");

            await contacts.AddPhoneAsync(phone, idContact);
        }

        private async Task AddEmailToContact(IContactStore contacts)
        {
            int idContact = int.Parse(ReadWithLabel("Enter Id of contact: "));
            string email = ReadWithLabel("Enter email:  ");

            await contacts.AddEmailAsync(email, idContact);
        }

        private string ReadWithLabel(string label)
        {
            Console.WriteLine(label);
            return Console.ReadLine();
        }
    }
}
