using FastEndpoints;
using Sensor6ty.Modules;
using Soditech.IntelPrev.Preventions.Application;
using Soditech.IntelPrev.Preventions.Persistence;

namespace Soditech.IntelPrev.Preventions.WebApi;

public class PreventionModuleInitializer : DefaultModuleInitializer
{
    public override void Initialize(WebApplicationBuilder builder, IConfiguration moduleConfiguration)
    {
        base.Initialize(builder, moduleConfiguration);

        builder.Services.AddPersistenceServices(moduleConfiguration);

        builder.Services.AddPreventionServices();

        builder.Services.AddFastEndpoints();
    }
}