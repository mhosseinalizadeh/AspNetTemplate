using AspNetTemplate.ClientEntity.DTO;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTemplate.ApplicationService.AccountService
{
    public interface IAccountService
    {
        Task<ServiceResult> AddExpense(ExpenseUploadDto model);
        Task<ServiceResult> LoadAllUserExpenses(int userid);
    }
}
