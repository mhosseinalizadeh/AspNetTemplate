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

        public Task<IEnumerable<User>> AllAsync()
        {
            var sql = $"SELECT FirstName, LastName FROM [{_tblName}]";
            return QueryAsync<User>(sql, null);
        }

        public Task<User> FindAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> FindByMailAsync(string email)
        {
            //string query = $"SELECT * FROM {_tblName} WHERE Email = @email";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@email", email, DbType.String, ParameterDirection.Input);
            //return QuerySingleOrDefaultAsync<User>(query, parameter);



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
                    userDic.Add(user.Id, userEntry);
                }

                return userEntry;
            }, parameter, "id, id, id");
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
