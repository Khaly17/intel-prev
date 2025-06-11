using Soditech.IntelPrev.Emails.Helpers;
using Soditech.IntelPrev.Emails.Services;
using Soditech.IntelPrev.Emails.Services.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Application;

namespace Soditech.IntelPrev.Emails.MailService;

public static class DependencyInjection
{
    public static IServiceCollection AddMailServerServices(this IServiceCollection services)
    {
        services.AddApplicationServices(typeof(DependencyInjection).Assembly);

        #region Mail services

        services.AddScoped<IEmailTemplateRenderer, EmailTemplateRenderer>();
#if DEBUG
        services.AddTransient<IEmailSender, NullEmailSender>();
#else 
        services.AddTransient<IEmailSender, EmailSender>();
#endif

        var builder = services.BuildServiceProvider();
        var configuration = builder.GetRequiredService<IConfiguration>();

        services.AddOptions<EwsOptions>()
            .Bind(configuration.GetSection(nameof(EwsOptions)))
            .ValidateOnStart();

        #endregion
        
        return services;
    }
}