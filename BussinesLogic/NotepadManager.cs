using ContactBookX.models;
using ContactBookX.storage;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookX.BussinesLogic
{
    class NotepadManager
    {
        private readonly IUserStore _userStore;
        private Dictionary<string,UserInfo> _accountsData;

        public NotepadManager(IUserStore userStore)
        {
            this._userStore = userStore;
            _accountsData = new Dictionary<string, UserInfo>();
        }

        public async Task AddUser(string username, string password, string firstname, string lastname)
        {

            if (_accountsData.Count == 0)
            {
                await CacheUsersData();
            }

            if (_accountsData.ContainsKey(username))
            {
                throw new InvalidOperationException(" The username is already exist: ");
            }

            UserInfo user = new UserInfo()
            {
                UserName = username,
                Password = password,
                FirstName = firstname,
                LastName = lastname
            };
            await _userStore.AddUserAsync(user);
        }

        public async Task AddContactToUser(ContactInfo contact, IContactStore contacts)
        {
            await contacts.AddContactAsync(contact);
        }
        
        public async Task AddPhoneToContact(string phone, int contactId, IContactStore contacts)
        {
            await contacts.AddPhoneAsync(phone, contactId);
        }
        
        public async Task AddEmailToContact(string email, int contactId, IContactStore contacts)
        {
            await contacts.AddEmailAsync(email, contactId);
        }

        public async Task<IContactStore> SignIn(string username, string password)
        {
            bool isUsernameExist = await ValidateUsername(username);

            if (isUsernameExist && _accountsData[username].Password == password)
            {
                return new DapperSQLContactStore(_accountsData[username].IdUser,
                                                 _userStore.ConntectionString);
            }

            throw new InvalidOperationException("Incorrect  username or password");
        }

        public async Task<string> ShowContacts(IContactStore contacts)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Contacts:  \n");
            int index = 1;

            var contactList = await contacts.GetContactsAsync();
            foreach (var contact in contactList)
            {
                sb.Append($"{index} :  \n{contact} \n");
                index++;
            }

            return sb.ToString();
        }

        public async Task<bool> ValidateUsername(string username)
        { 
            if (_accountsData.Count == 0)
            {
                await CacheUsersData();
            }

            bool isUsernameExist = _accountsData.ContainsKey(username);
            return isUsernameExist;
        }
        private async Task CacheUsersData()
        {
            var users = await _userStore.GetUsersAsync();
            _accountsData = users.ToDictionary(u => u.UserName, u => u);
        }
    }
}
