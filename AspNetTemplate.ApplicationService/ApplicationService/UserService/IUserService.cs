using AspNetTemplate.DomainEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTemplate.ApplicationService.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
    }
}
