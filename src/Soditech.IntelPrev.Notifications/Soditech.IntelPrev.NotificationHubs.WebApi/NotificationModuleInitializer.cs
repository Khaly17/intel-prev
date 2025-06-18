using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Sensor6ty.Modules;
using Soditech.IntelPrev.NotificationHubs.Application;

namespace Soditech.IntelPrev.NotificationHubs.WebApi;
public class NotificationModuleInitializer : DefaultModuleInitializer
{
    public override void Initialize(WebApplicationBuilder builder, IConfiguration moduleConfiguration)
    {
        base.Initialize(builder, moduleConfiguration);

        builder.Services.AddNotificationServices(moduleConfiguration);

        builder.Services.AddFastEndpoints();
    }
}