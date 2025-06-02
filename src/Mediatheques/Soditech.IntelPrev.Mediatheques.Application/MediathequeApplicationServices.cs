using Microsoft.Extensions.DependencyInjection;
using Sensor6ty.Application;
using Soditech.IntelPrev.Mediatheques.Application.Services;

namespace Soditech.IntelPrev.Mediatheques.Application;

public static class MediathequeApplicationServices
{
    public static IServiceCollection
        AddMediathequeServices(this IServiceCollection services)
    {
        services.AddApplicationServices(typeof(MediathequeApplicationServices).Assembly);
        
        services.AddScoped<IFileService, FileService>();

        return services;
    }
}