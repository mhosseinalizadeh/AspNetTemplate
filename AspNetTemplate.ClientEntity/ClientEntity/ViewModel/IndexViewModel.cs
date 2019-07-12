using AspNetTemplate.DomainEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.ClientEntity.ViewModel
{
    public class IndexViewModel : BaseViewModel
    {
        public IEnumerable<User> Users;
    }
}
