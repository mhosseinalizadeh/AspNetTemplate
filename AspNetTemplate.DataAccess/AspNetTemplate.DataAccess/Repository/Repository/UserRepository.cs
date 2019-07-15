using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DomainEntity;
using Dapper;
using PropSearch.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AspNetTemplate.DataAccess.Repository.Repository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private const string _tblName = "[User]";
        public UserRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public Task AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsyncById(User entity)
        {
            var sql = $"INSERT INTO {_tblName} (FirstName, LastName, Email, Password) Values " +
                $"(@firstName, @lastName, @email, @password) SELECT CAST(SCOPE_IDENTITY() as int) ";

            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@firstName", entity.FirstName, DbType.String, ParameterDirection.Input);
            parameter.Add("@lastName", entity.LastName, DbType.String, ParameterDirection.Input);
            parameter.Add("@email", entity.Email, DbType.String, ParameterDirection.Input);
            parameter.Add("@password", entity.Password, DbType.String, ParameterDirection.Input);

            return ExecuteScalarAsync<int>(sql, parameter);
        }

        public Task<IEnumerable<User>> AllAsync()
        {
            var sql = $"SELECT FirstName, LastName FROM {_tblName}";
            return QueryAsync<User>(sql, null);
        }

        public Task<User> FindAsync(int key)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@key", key, DbType.Int32, ParameterDirection.Input);

            var sql = $"SELECT * FROM {_tblName} " +
                $"WHERE Id = @key";

            return QuerySingleOrDefaultAsync<User>(sql, parameter);
        }

        public Task<IEnumerable<User>> FindByMailAsync(string email)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@email", email, DbType.String, ParameterDirection.Input);

            string query = $"SELECT * FROM {_tblName} AS u " +
                            $"LEFT JOIN UserRole AS ur ON u.id = ur.userId " +
                            $"LEFT JOIN Role as r ON r.id = ur.roleId " +
                            $"WHERE u.Email = @email";

            var userDic = new Dictionary<int, User>();
            return QueryAsync<User, UserRole, Role>(query, (user, userole, role) => {

                User userEntry;
                if (!userDic.TryGetValue(user.Id, out userEntry))
                {
                    userEntry = user;
                    userEntry.Roles = new List<Role>();

                    if (userole != null && !user.Roles.Any(c => c.Id == userole.RoleId) && role != null)
                        userEntry.Roles.Add(role);

                    userDic.Add(user.Id, userEntry);
                }

                return userEntry;
            }, parameter, "id, id, id");
        }

        public Task<IEnumerable<User>> FindFinanceUser()
        {
            int roleId = 3;
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@roleId", roleId, DbType.Int32, ParameterDirection.Input);

            string query = $"SELECT * FROM {_tblName} AS u " +
                            $"LEFT JOIN UserRole AS ur ON u.id = ur.userId " +
                            $"WHERE ur.RoleId = @roleId";

            var userDic = new Dictionary<int, User>();
            return QueryAsync<User, UserRole>(query, (user, userole) => {

                User userEntry;
                if (!userDic.TryGetValue(user.Id, out userEntry))
                {
                    userEntry = user;

                    userDic.Add(user.Id, userEntry);
                }

                return userEntry;
            }, parameter, "id, id");
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
