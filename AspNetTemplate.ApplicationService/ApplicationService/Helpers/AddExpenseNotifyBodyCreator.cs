using AspNetTemplate.ClientEntity.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTemplate.ApplicationService.Helpers
{
    public class AddExpenseNotifyBodyCreator : INotifyBodyCreator
    {
        public string CreateBody(NotifyBodyDto notifyBody)
        {
            var htmlStr = "";
            htmlStr += $"<div>An expense file uploaded by {notifyBody.User.FirstName} {notifyBody.User.LastName} at {notifyBody.ExpenseModel.UploadDate}.</div>" +
                $"<div>View uploaded expence: <a target='_blank' href='{notifyBody.AbsoluteUrl}/account/viewexpensefile/{notifyBody.ExpenseModel.Id}'>View File</a></div>";

            return htmlStr;
        }
    }
}
