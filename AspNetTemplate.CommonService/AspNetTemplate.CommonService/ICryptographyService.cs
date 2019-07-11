using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.CommonService
{
    public interface ICryptographyService
    {
       string ComputeHash (string data);
    }
}
