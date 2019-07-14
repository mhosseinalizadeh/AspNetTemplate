using AspNetTemplate.DomainEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTemplate.DataAccess.Repository.IRepository
{
    public interface IExpenseInfoRepository : IRepository<ExpenseInfo, int>
    {
        Task<IEnumerable<ExpenseInfo>> LoadAllUserExpenses(int userid);
        Task<int> AddAsyncById(ExpenseInfo entity);
    }
}
