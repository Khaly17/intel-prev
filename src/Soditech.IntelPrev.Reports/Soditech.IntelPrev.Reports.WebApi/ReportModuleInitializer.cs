using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Sensor6ty.Modules;
using Soditech.IntelPrev.Reports.Application.ReportAttachments;
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