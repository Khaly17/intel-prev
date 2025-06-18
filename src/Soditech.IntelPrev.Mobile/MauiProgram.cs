using System;
using CommunityToolkit.Maui;
using Controls.UserDialogs.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Sensor6ty.System.Reflection;
using Soditech.IntelPrev.Mobile.Core.DataStorage;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.Services.Account;
using Soditech.IntelPrev.Mobile.Services.Account.Models;
using Soditech.IntelPrev.Mobile.Services.Notifications;
using Soditech.IntelPrev.Mobile.Services.Settings;
using Soditech.IntelPrev.Mobile.Services.Storage;
using Soditech.IntelPrev.Proxy;
using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;


namespace Soditech.IntelPrev.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureSyncfusionCore()
            .UseUserDialogs(() => { })
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseSentry(options =>
            {
                // The DSN is the only required setting.
                options.Dsn = Config.SentryDsn;

                // Use debug mode if you want to see what the SDK is doing.
                // Debug messages are written to stdout with Console.Writeline,
                // and are viewable in your IDE's debug console or with 'adb logcat', etc.
                // This option is not recommended when deploying your application.
                options.Debug = false;

                // Set TracesSampleRate to 1.0 to capture 100% of transactions for tracing.
                // We recommend adjusting this value in production.
                options.TracesSampleRate = 1.0;

                options.MinimumEventLevel = LogLevel.Warning;
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
#if DEBUG
                logging.AddDebug();
#endif
                logging.AddSentry();// Send logs to Sentry

                logging.SetMinimumLevel(LogLevel.Information);
            });
                

        SyncfusionLicenseProvider
            .RegisterLicense(Config.SyncfusionLicense);



        builder.Services.AddSingleton<IAccountService, AccountService>(provider =>
        {
            var accountService = new AccountService(provider)
            {
                AuthenticateModel = new AuthenticateModel()
            };
            return accountService;
        });
        builder.Services.AddSingleton<IAccessTokenManager, AccessTokenManager>();
        builder.Services.AddSingleton<IDataStorageService, DataStorageService>();
        builder.Services.AddSingleton<IDataStorageManager, DataStorageManager>();
        // Register Settings Manager service
        builder.Services.AddSingleton<ISettingsManager, SettingsManager>();

        builder.Services.AddAutoMapper(typeof(MauiProgram).GetAssembly());

        builder.Services.AutoRegisterServices(typeof(MauiProgram).GetAssembly());

        builder.Services.AddScoped<IProxyService, ProxyService>();

        builder.RegisterServices();

        return builder.Build();
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
#if IOS
            builder.Services.AddSingleton<IDeviceInstallationService, DeviceiOsInstallationService>();
#elif ANDROID
        builder.Services.AddSingleton<IDeviceInstallationService, DeviceAndroidInstallationService>();
#endif

        builder.Services.AddSingleton<IPushDemoNotificationActionService, PushDemoNotificationActionService>();
        builder.Services.AddSingleton<INotificationRegistrationService, NotificationRegistrationService>();

        return builder;
    }


    private static MauiAppBuilder ConfigureLogging(this MauiAppBuilder builder, Action<ILoggingBuilder> configure)
    {
        configure.Invoke(builder.Logging);
        return builder;
    }
}