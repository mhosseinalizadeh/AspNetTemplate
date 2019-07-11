using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.CommonService
{
    public interface ICacheService
    {
        void Set(string key, object value);
        object Get(string key);
    }
}
