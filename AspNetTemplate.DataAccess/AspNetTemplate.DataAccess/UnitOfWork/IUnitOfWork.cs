using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
