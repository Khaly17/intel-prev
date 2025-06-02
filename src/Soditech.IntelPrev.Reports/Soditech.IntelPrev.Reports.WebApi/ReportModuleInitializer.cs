using FastEndpoints;
using Sensor6ty.Modules;
using Soditech.IntelPrev.Reports.Application;
using Soditech.IntelPrev.Reports.Persistence;

namespace Soditech.IntelPrev.Reports.WebApi;

public class ReportModuleInitializer : DefaultModuleInitializer
{
    public override void Initialize(WebApplicationBuilder builder, IConfiguration moduleConfiguration)
    {
        base.Initialize(builder, moduleConfiguration);

        builder.Services.AddPersistenceServices(moduleConfiguration);

        builder.Services.AddReportServices();

        builder.Services.AddFastEndpoints();
    }
}