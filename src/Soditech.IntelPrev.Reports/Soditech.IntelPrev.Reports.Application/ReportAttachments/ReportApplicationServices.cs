using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Application;

namespace Soditech.IntelPrev.Reports.Application.ReportAttachments;

public static class ReportApplicationServices
{
    public static IServiceCollection
        AddReportServices(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddApplicationServices(typeof(ReportApplicationServices).Assembly);

        return services;
    }
}