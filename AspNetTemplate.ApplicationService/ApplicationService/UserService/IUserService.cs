using AspNetTemplate.ClientEntity.DTO;
using AspNetTemplate.DomainEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTemplate.ApplicationService.UserService
{
    public interface IUserService
    {
        Task<ServiceResult> GetAllUsers();
        Task<ServiceResult> CheckPassword(LoginDto loginModel);
    }
}
