using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DomainEntity;
using PropSearch.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTemplate.DataAccess.Repository.Repository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private const string _tableName = "User";
        public UserRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public Task AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> AllAsync()
        {
            var sql = $"SELECT FirstName, LastName FROM [{_tableName}]";
            return QueryAsync<User>(sql, null);
        }

        public Task<User> FindAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
