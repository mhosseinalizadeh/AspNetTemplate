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
    public class UserRoleRepository : RepositoryBase, IUserRoleRepository
    {
        private const string _tblName = "UserRole";
        public UserRoleRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public Task AddAsync(UserRole entity)
        {
            var sql = $"INSERT INTO {_tblName} (UserId, RoleId) Values (@userId, @roleID)";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@userId", entity.UserId, DbType.Int32, ParameterDirection.Input);
            parameter.Add("@roleID", entity.RoleId, DbType.Int32, ParameterDirection.Input);

            return ExecuteScalarAsync<UserRole>(sql, parameter);
        }

        public Task<IEnumerable<UserRole>> AllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserRole> FindAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
