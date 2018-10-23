using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fosol.Schedule.API.Helpers.Mail
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMailClient(this IServiceCollection services, Action<MailOptions> setupAction)
        {
            var options = new MailOptions();
            setupAction?.Invoke(options);
            return services.AddSingleton<MailClient>();
        }

        public static IServiceCollection AddMailClient(this IServiceCollection services)
        {
            return services.AddSingleton<MailClient>();
        }
    }
}
