using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;

namespace Soditech.IntelPrev.Mobile.Core.Dependency;

public static class DependencyResolver
{
    // Static property to hold the IServiceProvider instance
    private static IServiceProvider? _serviceProvider = IPlatformApplication.Current?.Services;

    // This method should be called once, typically in the App.xaml.cs or MauiProgram.cs
    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    // Static method to get the required service
    public static T GetRequiredService<T>() where T : notnull
    {
        if (_serviceProvider == null)
            throw new InvalidOperationException("DependencyResolver is not initialized. Call Initialize first.");

        return _serviceProvider.GetRequiredService<T>();
    }
    
}

public interface ITransientDependency
{
}

public static class ServiceRegistration
{
    public static void AutoRegisterServices(this IServiceCollection services, Assembly assembly)
    {
        var serviceTypes = assembly.GetTypes().Where(t =>
            t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(ITransientDependency)) && !t.IsAbstract)).ToList();

        foreach (var serviceType in serviceTypes)
        {
            // Register service with transient lifetime
            services.AddTransient(serviceType);
        }
    }
}
