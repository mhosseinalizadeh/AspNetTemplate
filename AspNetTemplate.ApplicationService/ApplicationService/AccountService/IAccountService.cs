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
        Task<ServiceResult> GetExpenseAsync(int id, int userid, string userRole);
        Task<ServiceResult> GetExpenseFile(int id, int userid, string userRole);
        Task<ServiceResult> LoadAllExpenses();
        Task<ServiceResult> AcceptExpense(int id, int userid, string userRole);
        Task<ServiceResult> DeclineExpense(int id, int userid, string userRole, string stateDescription);
        Task<ServiceResult> AddUser(UserDto userModel);
    }
}
