using AspNetTemplate.DomainEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTemplate.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<IEnumerable<User>> FindByMailAsync(string email);
    }
}
