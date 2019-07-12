using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DomainEntity;
using Dapper;
using PropSearch.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTemplate.DataAccess.Repository.Repository
{
    public class ExpenseInfoRepository : RepositoryBase, IExpenseInfoRepository
    {
        private const string _tblName = "[ExpenseInfo]";
        public ExpenseInfoRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public Task AddAsync(ExpenseInfo entity)
        {
            var sql = $"INSERT INTO {_tblName} VALUES (@path, @filename, @ownerid, @state, @description, @stateDescription, @uploadDate)";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@path", entity.Path, DbType.String, ParameterDirection.Input);
            parameter.Add("@filename", entity.FileName, DbType.String, ParameterDirection.Input);
            parameter.Add("@ownerid", entity.OwnerId, DbType.Int32, ParameterDirection.Input);
            parameter.Add("@state", entity.State, DbType.String, ParameterDirection.Input);
            parameter.Add("@description", entity.Description, DbType.String, ParameterDirection.Input);
            parameter.Add("@stateDescription", entity.StateDescription, DbType.String, ParameterDirection.Input);
            parameter.Add("@uploadDate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

            return ExecuteScalarAsync<ExpenseInfo>(sql, parameter);
        }

        public Task<IEnumerable<ExpenseInfo>> AllAsync()
        {
            var sql = $"SELECT * FROM {_tblName}";
            return QueryAsync<ExpenseInfo>(sql);
        }

        public Task<ExpenseInfo> FindAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExpenseInfo>> LoadAllUserExpenses(int userid)
        {

            var sql = $"SELECT * FROM {_tblName} " +
                $"WHERE OwnerId = @userid";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@userid", userid, DbType.Int32, ParameterDirection.Input);
            return QueryAsync<ExpenseInfo>(sql, parameter);
        }

        public Task RemoveAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ExpenseInfo entity)
        {
            throw new NotImplementedException();
        }
    }
}
