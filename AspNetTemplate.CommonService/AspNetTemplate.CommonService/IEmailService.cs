using System.Threading.Tasks;

namespace AspNetTemplate.CommonService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string message);
    }
}
