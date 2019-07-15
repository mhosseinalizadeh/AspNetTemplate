using AspNetTemplate.ApplicationService;
using AspNetTemplate.ApplicationService.AccountService;
using AspNetTemplate.ApplicationService.UserService;
using AspNetTemplate.CommonService;
using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DomainEntity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetTemplate.Test.ApplicationService
{
    [TestClass]
    public class TestUserService
    {
        Mock<IUserRepository> mockUserRepo = new Mock<IUserRepository>();
         Mock<ICryptographyService> mockCryptoService = new Mock<ICryptographyService>();
         Mock<ILocalizationService> mockLocService = new Mock<ILocalizationService>();
         

        [TestMethod]
        public async Task TestMethod_CheckPassword_UserNotFound()
        {
            UserService userService = new UserService(mockUserRepo.Object, mockCryptoService.Object, mockLocService.Object);
            mockUserRepo.Setup(repo => repo.FindByMailAsync(It.IsAny<string>()))
                .ReturnsAsync((IEnumerable<User>)null);
            mockCryptoService.Setup(cr => cr.ComputeHash(It.IsAny<string>())).Returns("123456");

            var res = await userService.CheckPassword(new SampleData().LoginDto);
            mockUserRepo.Verify(m => m.FindByMailAsync(It.IsAny<string>()), Times.Once);
            mockCryptoService.Verify(m => m.ComputeHash(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOfType(res, typeof(ServiceResult));
            Assert.AreEqual(res.Status, ServiceResultStatus.Exception);
        }

        [TestMethod]
        public async Task TestMethod_CheckPassword_PasswordWrong()
        {
            UserService userService = new UserService(mockUserRepo.Object, mockCryptoService.Object, mockLocService.Object);
            mockUserRepo.Setup(repo => repo.FindByMailAsync(It.IsAny<string>()))
                .ReturnsAsync(new SampleData().AllUsers);
            mockCryptoService.Setup(cr => cr.ComputeHash(It.IsAny<string>())).Returns("123");

            var res = await userService.CheckPassword(new SampleData().LoginDto);
            mockUserRepo.Verify(m => m.FindByMailAsync(It.IsAny<string>()), Times.Once);
            mockCryptoService.Verify(m => m.ComputeHash(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOfType(res, typeof(ServiceResult));
            Assert.AreEqual(res.Status, ServiceResultStatus.Exception);
        }
    }
}
