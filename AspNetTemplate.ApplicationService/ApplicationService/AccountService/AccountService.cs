using AspNetTemplate.ClientEntity.DTO;
using AspNetTemplate.CommonService;
using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DomainEntity;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetTemplate.ApplicationService.AccountService
{
    public class AccountService : IAccountService
    {
        private ILocalizationService _localizer;
        private IFileService _fileService;
        private IExpenseInfoRepository _expenseInfoRepository;
        public AccountService(ILocalizationService localizationService, 
            IFileService fileService,
            IExpenseInfoRepository expenseInfoRepository)
        {
            _localizer = localizationService;
            _fileService = fileService;
            _expenseInfoRepository = expenseInfoRepository;
        }

        public async Task<ServiceResult> AddExpense(ExpenseUploadDto model)
        {
            var filePath = _fileService.GetNewExpenseFilePath();

            if (model.ExpensePhoto.Length > 0)
            {
                var filename = Guid.NewGuid().ToString();
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ExpensePhoto.CopyToAsync(stream);
                }

                await _expenseInfoRepository.AddAsync(new ExpenseInfo()
                {
                    FileName = model.ExpensePhoto.FileName,
                    OwnerId = model.UserId,
                    Path = filename,
                    State = ExpenseState.UnApproved,
                    Description = model.Description
                });
            }

            return new ServiceResult(ServiceResultStatus.Success, null, _localizer.Localize("Your expense file was uploaded successfully."));

        }

        public async Task<ServiceResult> LoadAllUserExpenses(int userid)
        {
            var res = await _expenseInfoRepository.LoadAllUserExpenses(userid);
            return new ServiceResult(ServiceResultStatus.Success, res);
        }

        public async Task<JqueryDataTablesPagedResults<ExpenseDatatableDto>> LoadExpenses(JqueryDataTablesParameters param)
        {


            var size = 0;

            var res = await _expenseInfoRepository.AllAsync();

            size = res.Count();
            var dtoResult = res.Select(c => new ExpenseDatatableDto()
            {
                Description = c.Description,
                File = c.Path,
                FileName = c.FileName,
                Id = c.Id,
                State = c.State.ToString(),
                StateDescription = c.StateDescription,
                UploadDate = c.UploadDate
            });

            return new JqueryDataTablesPagedResults<ExpenseDatatableDto>
            {
                Items = dtoResult,
                TotalSize = size
            };
        }
    }
}
