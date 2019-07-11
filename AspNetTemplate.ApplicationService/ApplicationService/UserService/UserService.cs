using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetTemplate.ClientEntity.DTO;
using AspNetTemplate.CommonService;
using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DomainEntity;
using System.Linq;

namespace AspNetTemplate.ApplicationService.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptographyService _cryptographyService;
        private readonly ILocalizationService _localizationService;

        public UserService(IUserRepository userRepository,
            ICryptographyService cryptographyService,
            ILocalizationService localizationService)
        {
            _userRepository = userRepository;
            _cryptographyService = cryptographyService;
            _localizationService = localizationService;
        }

        public async Task<ServiceResult> GetAllUsers()
        {
            return new ServiceResult(ServiceResultStatus.Success,_userRepository.AllAsync());
        }

        public async Task<ServiceResult> CheckPassword(LoginDto loginModel)
        {
            var hashedPassword = getHashed(loginModel.Password);

            var user = (await _userRepository.FindByMailAsync(loginModel.Email)).FirstOrDefault();
            if(user == null)
                return new ServiceResult(ServiceResultStatus.Exception, null, _localizationService.Localize("User Not Found!"));

            if (user.Password != hashedPassword)
                return new ServiceResult(ServiceResultStatus.Exception, null, _localizationService.Localize("Password is wrong!"));

            return new ServiceResult(ServiceResultStatus.Success, user);
        }

        private string getHashed(string password)
        {
            return _cryptographyService.ComputeHash(password);
        }
    }
}
