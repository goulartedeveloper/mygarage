using System.Collections.Generic;
using System.Threading.Tasks;

namespace Garage.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string htmlBody, List<string> attachments = null);
    }
}
