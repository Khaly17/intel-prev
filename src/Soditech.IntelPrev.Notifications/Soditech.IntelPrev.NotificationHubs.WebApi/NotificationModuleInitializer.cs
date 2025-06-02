using FastEndpoints;
using Sensor6ty.Modules;
using Soditech.IntelPrev.NotificationHubs.Application;

public class NotificationModuleInitializer : DefaultModuleInitializer
{
    public override void Initialize(WebApplicationBuilder builder, IConfiguration moduleConfiguration)
    {
        base.Initialize(builder, moduleConfiguration);

        builder.Services.AddNotificationServices(moduleConfiguration);

        builder.Services.AddFastEndpoints();
    }
}