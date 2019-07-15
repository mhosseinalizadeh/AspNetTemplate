using AspNetTemplate.DataAccess.Repository.IRepository;
using AspNetTemplate.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetTemplate.CommonService
{
    public class LocalizationService : ILocalizationService
    {
        List<Localization> _localizations;
        public LocalizationService(ILocalizationRepository localizationRepository, ICacheService cacheService)
        {
            var locals = cacheService.Get(CacheKeys.Phrase);
            if (locals == null) {
                _localizations = localizationRepository.AllAsync("en").Result.ToList();
                if(_localizations.Count > 0)
                    cacheService.Set(CacheKeys.Phrase, _localizations);
            }
            else
                _localizations = (List<Localization>)locals;
        }

        public string Localize(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            var rec = _localizations.SingleOrDefault(c => c.Code == code);
            return rec != null ? rec.Text : code ;
        }
    }
}
