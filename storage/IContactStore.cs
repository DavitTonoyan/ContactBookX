using ContactBookX.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactBookX.storage
{
    interface IContactStore
    {
        Task AddContactAsync(ContactInfo contact);
        Task AddEmailAsync(string email, int contactId);
        Task AddPhoneAsync(string phone, int contactId);
        Task<IEnumerable<ContactInfo>> GetContactsAsync();
    }
}
