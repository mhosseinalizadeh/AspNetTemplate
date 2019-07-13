using AspNetTemplate.ClientEntity.DTO;
using AspNetTemplate.CommonService;
using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DomainEntity;
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

        public AccountService(ILocalizationService localizationService,
            IFileService fileService,
            IExpenseInfoRepository expenseInfoRepository,
            IEmailService emailService,
            IUserRepository userRepository)
        {
            _localizer = localizationService;
            _fileService = fileService;
            _expenseInfoRepository = expenseInfoRepository;
            _emailService = emailService;
            _userRepository = userRepository;
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
                    Description = model.Description
                };
                await _expenseInfoRepository.AddAsync(expense);

                var financeUser = await _userRepository.FindFinanceUser();
                var tasks = new List<Task>();
                foreach (var user in financeUser)
                {
                    var message = createMailBody(getFormattedExpense(expense));
                    tasks.Add(_emailService.SendEmailAsync(user.Email, _localizer.Localize("A spense submitted"), ""));
                }
                await Task.WhenAll(tasks);
            }

            return new ServiceResult(ServiceResultStatus.Success, null, _localizer.Localize("Your expense file was uploaded successfully."));

        }

        private string createMailBody(ExpenseDatatableDto expenseDatatableDto)
        {
            throw new NotImplementedException();
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

    }
}
