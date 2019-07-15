using AspNetTemplate.ApplicationService.Helpers;
using AspNetTemplate.ClientEntity;
using AspNetTemplate.ClientEntity.DTO;
using AspNetTemplate.CommonService;
using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DomainEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static AspNetTemplate.ClientEntity.Enums;

namespace AspNetTemplate.ApplicationService.AccountService
{
    public class AccountService : IAccountService
    {
        private ILocalizationService _localizer;
        private IFileService _fileService;
        private IEmailService _emailService;
        private IExpenseInfoRepository _expenseInfoRepository;
        private IUserRepository _userRepository;
        private IActionContextAccessor _actionContextAccessor;
        private readonly Func<NotifyType, INotifyBodyCreator> _notifyBodyCreatorAccessor;
        private ICryptographyService _cryptographyService;
        private IUserRoleRepository _userRoleRepository;

        public AccountService(ILocalizationService localizationService,
            IFileService fileService,
            IExpenseInfoRepository expenseInfoRepository,
            IEmailService emailService,
            IUserRepository userRepository,
            IActionContextAccessor actionContextAccessor,
            Func<NotifyType, INotifyBodyCreator> notifyBodyCreatorAccessor,
            ICryptographyService cryptographyService,
            IUserRoleRepository userRoleRepository)
        {
            _localizer = localizationService;
            _fileService = fileService;
            _expenseInfoRepository = expenseInfoRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _actionContextAccessor = actionContextAccessor;
            _notifyBodyCreatorAccessor = notifyBodyCreatorAccessor;
            _cryptographyService = cryptographyService;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<ServiceResult> AcceptExpense(int id, int userid, string userRole)
        {
            if (!hasManageExpensePermission(userRole))
                return new ServiceResult(ServiceResultStatus.Exception, null, _localizer.Localize("Permission Denied!"));

            var expense = await UpdateExpense(id, State.Approved, "");

            await notifyFinanceUser(NotifyType.ApproveExpense, expense, userid);

            return new ServiceResult(ServiceResultStatus.Success, null, _localizer.Localize("The expense approved successfully."));
        }

        public async Task<ServiceResult> DeclineExpense(int id, int userid, string userRole, string stateDescription)
        {
            if (!hasManageExpensePermission(userRole))
                return new ServiceResult(ServiceResultStatus.Exception, null, _localizer.Localize("Permission Denied!"));

            var expense = await UpdateExpense(id, State.Declined, stateDescription);

            await notifyFinanceUser(NotifyType.DeclineExpense, expense, userid);

            return new ServiceResult(ServiceResultStatus.Success, null, _localizer.Localize("The expense declined!"));
        }

        public async Task<ServiceResult> AddExpense(ExpenseUploadDto model)
        {
            if (model.ExpensePhoto.Length > 0)
            {
                var expense = await addExpense(model);
                await notifyFinanceUser(NotifyType.AddExpense,  expense, model.UserId);
            }

            return new ServiceResult(ServiceResultStatus.Success, null, _localizer.Localize("Your expense file was uploaded successfully."));
        }

        public async Task<ServiceResult> GetExpenseAsync(int id, int userid, string userRole)
        {
            var expense = await _expenseInfoRepository.FindAsync(id);
            if (expense.OwnerId == userid || userRole != "Employee")
            {
                return new ServiceResult(ServiceResultStatus.Success, expense);
            }

            return new ServiceResult(ServiceResultStatus.Exception, null, _localizer.Localize("Permission Denied!"));
        }

        public async Task<ServiceResult> GetExpenseFile(int id, int userid, string userRole)
        {
            var expense = await _expenseInfoRepository.FindAsync(id);
            if (expense != null && (expense.OwnerId == userid || userRole != "Employee"))
            {
                expense.Content = await File.ReadAllBytesAsync(expense.Path);
                return new ServiceResult(ServiceResultStatus.Success, expense);
            }

            return new ServiceResult(ServiceResultStatus.Exception, null, _localizer.Localize("Permission Denied!"));
        }

        public async Task<ServiceResult> LoadAllExpenses()
        {
            var res = await _expenseInfoRepository.AllAsync();
            var formattedData = getFormattedExpenses(res);
            return new ServiceResult(ServiceResultStatus.Success, formattedData);
        }

        public async Task<ServiceResult> LoadAllUserExpenses(int userid)
        {
            var res = await _expenseInfoRepository.LoadAllUserExpenses(userid);
            var formattedData = getFormattedExpenses(res);
            return new ServiceResult(ServiceResultStatus.Success, formattedData);
        }

        public async Task<ServiceResult> AddUser(UserDto userModel)
        {
            userModel.Id = await _userRepository.AddAsyncById(new User {
                Email = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Password = _cryptographyService.ComputeHash(userModel.Password)
            });

            await _userRoleRepository.AddAsync(new UserRole() { RoleId = (int)userModel.Role, UserId = userModel.Id });

            return new ServiceResult(ServiceResultStatus.Success, null, _localizer.Localize("New user created successfully."));
        }

        #region private methods
        private List<ExpenseDto> getFormattedExpenses(IEnumerable<ExpenseInfo> res)
        {
            var list = new List<ExpenseDto>();
            foreach (var item in res)
            {
                list.Add(getFormattedExpense(item));
            }
            return list;
        }

        private ExpenseDto getFormattedExpense(ExpenseInfo item) {
            var expenseDto = new ExpenseDto()
            {
                Description = item.Description,
                FileName = item.FileName,
                Id = item.Id,
                Link = $"/account/viewexpensefile/{item.Id}",
                State = item.State,
                StateDescription = item.StateDescription,
                UploadDate = item.UploadDate.ToString("MM/dd/yyy HH:mm:ss")
            };
            return expenseDto;
        }

        private string GetAbsoluteUrl(HttpRequest request)
        {
            return string.Concat(
                        request.Scheme,
                        "://",
                        request.Host.ToUriComponent(),
                        request.PathBase.ToUriComponent());
        }

        private bool hasManageExpensePermission(string userRole)
        {
            return userRole == "TeamLead";
        }

        private async Task<ExpenseInfo> UpdateExpense(int id, State state, string stateDescription)
        {
            var expense = await _expenseInfoRepository.FindAsync(id);
            expense.State = state.GetDescription();
            expense.StateDescription = stateDescription;

            await _expenseInfoRepository.UpdateAsync(expense);
            return expense;
        }

        private async Task notifyFinanceUser(NotifyType type, ExpenseInfo expense, int userId)
        {
            var financeUsers = await _userRepository.FindFinanceUser();
            var user = await _userRepository.FindAsync(userId);
            var notifyBody = new NotifyBodyDto()
            {
                ExpenseModel = getFormattedExpense(expense),
                User = user,
                AbsoluteUrl = getAbsoluteUrl()
            };
            INotifyBodyCreator bodyCreator = _notifyBodyCreatorAccessor(type);
            var message = bodyCreator.CreateBody(notifyBody);

            var subject = getNotifySubjectbyType(type);

            var notifyData = new NotifyDataDto()
            {
                To = financeUsers,
                Message = message,
                Subject = _localizer.Localize(subject)
            };

            await notifyUser(notifyData);
        }

        private string getNotifySubjectbyType(NotifyType type)
        {
            switch (type)
            {
                case NotifyType.AddExpense:
                    return "A Spense Added";
                case NotifyType.DeclineExpense:
                    return "A Spense Declined";
                case NotifyType.ApproveExpense:
                    return "A Spense Approved";
                default:
                    return string.Empty;

            }
        }

        private async Task<ExpenseInfo> addExpense(ExpenseUploadDto model)
        {
            var filePath = _fileService.GetNewExpenseFilePath();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ExpensePhoto.CopyToAsync(stream);
            }
            var expense = new ExpenseInfo()
            {
                FileName = model.ExpensePhoto.FileName,
                OwnerId = model.UserId,
                Path = filePath,
                State = ExpenseState.UnApproved.ToString(),
                UploadDate = DateTime.Now,
                Description = model.Description
            };
            expense.Id = await _expenseInfoRepository.AddAsyncById(expense);

            return expense;
        }

        private async Task notifyUser(NotifyDataDto notifyData)
        {
            var tasks = new List<Task>();

            foreach (var notifyUser in notifyData.To)
                tasks.Add(_emailService.SendEmailAsync(notifyUser.Email, notifyData.Subject, notifyData.Message));

            await Task.WhenAll(tasks);
        }

        private string getAbsoluteUrl()
        {
            var request = _actionContextAccessor.ActionContext.HttpContext.Request;
            return GetAbsoluteUrl(request);
        }
#endregion private methods

    }
}
