using ContactBookX.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactBookX.storage
{
    interface IUserStore
    {
        string ConntectionString { get; }
        Task<IEnumerable<UserInfo>> GetUsersAsync();
        Task AddUserAsync(UserInfo user);
    }
}
