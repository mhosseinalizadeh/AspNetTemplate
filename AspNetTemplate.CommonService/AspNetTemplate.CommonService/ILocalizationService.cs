﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.CommonService
{
    public interface ILocalizationService
    {
        string Localize(string code);
    }
}
