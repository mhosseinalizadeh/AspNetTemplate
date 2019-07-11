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
    public class LocalizationRepository : RepositoryBase, ILocalizationRepository
    {
        private const string _tblName = "Localization";
        public LocalizationRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public Task AddAsync(Localization entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Localization>> AllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Localization>> AllAsync(string locale)
        {
            string query = $"SELECT * FROM {_tblName} WHERE Locale = @locale";
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add("@locale", locale, DbType.String, ParameterDirection.Input);
            return QueryAsync<Localization>(query, parameter);
        }

        public Task<Localization> FindAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Localization entity)
        {
            throw new NotImplementedException();
        }
    }
}
