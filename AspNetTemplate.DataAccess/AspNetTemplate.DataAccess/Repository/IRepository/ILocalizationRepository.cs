using AspNetTemplate.DomainEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTemplate.DataAccess.Repository.IRepository
{
    public interface ILocalizationRepository:IRepository<Localization, int>
    {
        Task<IEnumerable<Localization>> AllAsync(string locale);
    }
}
