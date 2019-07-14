using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetTemplate.ClientEntity.DTO;

namespace AspNetTemplate.ApplicationService.Helpers
{
    public interface INotifyBodyCreator
    {
        string CreateBody(NotifyBodyDto notifyBody);
    }
}
