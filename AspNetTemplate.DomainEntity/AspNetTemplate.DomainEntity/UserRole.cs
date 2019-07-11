using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.DomainEntity
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
