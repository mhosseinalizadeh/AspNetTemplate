using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.DomainEntity
{
    public class Localization
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Text { get; set; }
        public string Local { get; set; }
    }
}
