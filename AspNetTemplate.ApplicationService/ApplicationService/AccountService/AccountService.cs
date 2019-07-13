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

        public AccountService(ILocalizationService localizationService,
            IFileService fileService,
            IExpenseInfoRepository expenseInfoRepository,
            IEmailService emailService,
            IUserRepository userRepository,
            IActionContextAccessor actionContextAccessor)
        {
            _localizer = localizationService;
            _fileService = fileService;
            _expenseInfoRepository = expenseInfoRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<ServiceResult> AcceptExpense(int id, int userid, string userRole)
        {
            if (userRole != "TeamLead")
            {
                return new ServiceResult(ServiceResultStatus.Exception, null, _localizer.Localize("Permission Denied!"));
            }

            var expense = await _expenseInfoRepository.FindAsync(id);

            expense.State = "Approved";
            expense.StateDescription = "";
            await _expenseInfoRepository.UpdateAsync(expense);

            return new ServiceResult(ServiceResultStatus.Success, null, _localizer.Localize("The expense approved successfully."));
        }

        public async Task<ServiceResult> DeclineExpense(int id, int userid, string userRole, string stateDescription)
        {
            if (userRole != "TeamLead")
            {
                return new ServiceResult(ServiceResultStatus.Exception, null, _localizer.Localize("Permission Denied!"));
            }

            var expense = await _expenseInfoRepository.FindAsync(id);

            expense.State = "Declined";
            expense.StateDescription = stateDescription;

            await _expenseInfoRepository.UpdateAsync(expense);

            return new ServiceResult(ServiceResultStatus.Success, null, _localizer.Localize("The expense declined!"));
        }

        public async Task<ServiceResult> AddExpense(ExpenseUploadDto model)
        {
            var filePath = _fileService.GetNewExpenseFilePath();

            if (model.ExpensePhoto.Length > 0)
            {

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
                await _expenseInfoRepository.AddAsync(expense);

                var user = await _userRepository.FindAsync(model.UserId);

                var financeUsers = await _userRepository.FindFinanceUser();
                var tasks = new List<Task>();
                foreach (var financeUser in financeUsers)
                {
                    var message = createMailBody(getFormattedExpense(expense), user);
                    tasks.Add(_emailService.SendEmailAsync(financeUser.Email, _localizer.Localize("A spense submitted"), message));
                }
                await Task.WhenAll(tasks);
            }

            return new ServiceResult(ServiceResultStatus.Success, null, _localizer.Localize("Your expense file was uploaded successfully."));

        }

        private string createMailBody(ExpenseDatatableDto expenseDatatableDto, User user)
        {

            var request = _actionContextAccessor.ActionContext.HttpContext.Request;
            var absoluteUrl = GetAbsoluteUrl(request);

            var htmlStr = "";
            htmlStr += $"<div>An expense file uploaded by {user.FirstName} {user.LastName} at {expenseDatatableDto.UploadDate}.</div>" +
                $"<div>View uploaded expence: <a target='_blank' href='{absoluteUrl}/account/viewexpense/{expenseDatatableDto.Id}'>View File</a></div>";

            return htmlStr;
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

        private List<ExpenseDatatableDto> getFormattedExpenses(IEnumerable<ExpenseInfo> res)
        {
            var list = new List<ExpenseDatatableDto>();
            foreach (var item in res)
            {
                list.Add(getFormattedExpense(item));
            }
            return list;
        }

        private ExpenseDatatableDto getFormattedExpense(ExpenseInfo item) {
            var expenseDto = new ExpenseDatatableDto()
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

    }
}
