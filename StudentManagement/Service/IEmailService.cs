using StudentManagement.Models;
using System.Threading.Tasks;

namespace StudentManagement.Service
{
    public interface IEmailService
    {
        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);

        Task SendEmailForForgettenPassword(UserEmailOptions userEmailOptions);
    }
}