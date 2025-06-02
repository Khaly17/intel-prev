using FastEndpoints;
using Sensor6ty.Modules;
using Soditech.IntelPrev.Mediatheques.Application;
using Soditech.IntelPrev.Mediatheques.Persistence;

namespace Soditech.IntelPrev.Mediatheques.WebApi;

public class MediathequeModuleInitializer : DefaultModuleInitializer
{
    public override void Initialize(WebApplicationBuilder builder, IConfiguration moduleConfiguration)
    {
        base.Initialize(builder, moduleConfiguration);

        builder.Services.AddPersistenceServices(moduleConfiguration);

        builder.Services.AddMediathequeServices();

        //builder.Services.AddFastEndpoints();
    }
}