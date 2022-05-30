using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.DTOs.Email;

namespace TechTest.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
