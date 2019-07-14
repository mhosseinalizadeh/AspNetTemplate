using AspNetTemplate.DomainEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.ClientEntity.DTO
{
    public class NotifyDataDto
    {
        public int UserId { get; set; }
        public ExpenseInfo Expense { get; set; }
        public IEnumerable<User> To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
