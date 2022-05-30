using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechTest.Application.Interfaces;
using TechTest.Domain.Settings;
using TechTest.Infrastructure.Shared.Services;

namespace TechTest.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
