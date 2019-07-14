using AspNetTemplate.DomainEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.ClientEntity.DTO
{
    public class NotifyBodyDto
    {
        public User User { get; set; }
        public ExpenseDto ExpenseModel { get; set; }
        public string AbsoluteUrl { get; set; }
    }
}
