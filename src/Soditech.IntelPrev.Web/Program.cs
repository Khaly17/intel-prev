using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Soditech.IntelPrev.Web;
using Soditech.IntelPrev.Web.Events;
using Soditech.IntelPrev.Web.Services.Alert;
using Soditech.IntelPrev.Web.Services.Authentications;
using Soditech.IntelPrev.Web.Services.Authentications.Extensions;
using Soditech.IntelPrev.Web.Services.Cache;
using Soditech.IntelPrev.Web.Services.Helper;
using Soditech.IntelPrev.Web.Services.Proxy;
using Soditech.IntelPrev.Web.Services.UserInfo;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


// Register Syncfusion license
Syncfusion
    .Licensing
    .SyncfusionLicenseProvider
    .RegisterLicense(AppConstants.SyncfusionLicenseKey);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddOpenIddictServices();

builder.Services
    .AddHttpClient<IProxyService, ProxyService>(client =>
    {
        client.BaseAddress = new Uri($"{builder.Configuration["HostApi:BaseUri"]}");
    })
    .AddBearerTokenHandler();

builder.Services.AddScoped<ICookieStorageAccessor, CookieStorageAccessor>();
builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
builder.Services.AddScoped<IUserInfoService, UserInfoService>();
builder.Services.AddScoped<NavigationService>();
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddSingleton<SiteEventService>();

await builder.Build().RunAsync();