using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DomainEntity;

namespace AspNetTemplate.ApplicationService.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<IEnumerable<User>> GetAllUsers()
        {
            return _userRepository.AllAsync();
        }
    }
}
