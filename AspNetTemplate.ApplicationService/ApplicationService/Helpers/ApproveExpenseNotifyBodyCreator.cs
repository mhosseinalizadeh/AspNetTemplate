using System;
using System.Collections.Generic;
using System.Text;
using AspNetTemplate.ClientEntity.DTO;

namespace AspNetTemplate.ApplicationService.Helpers
{
    public class ApproveExpenseNotifyBodyCreator : INotifyBodyCreator
    {
        public string CreateBody(NotifyBodyDto notifyBody)
        {
            var htmlStr = "";
            htmlStr += $"<div>An expense file by Id {notifyBody.ExpenseModel.Id} for {notifyBody.User.FirstName} {notifyBody.User.LastName} <b>was Approved</b></div>" +
                $"<div>View approved expense: <a target='_blank' href='{notifyBody.AbsoluteUrl}/account/viewexpense/{notifyBody.ExpenseModel.Id}'>View File</a></div>";

            return htmlStr;
        }
    }
}
