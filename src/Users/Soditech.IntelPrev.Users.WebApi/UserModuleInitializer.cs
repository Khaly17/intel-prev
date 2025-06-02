using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using Sensor6ty.Modules;
using Sensor6ty.WebApi;
using Soditech.IntelPrev.Emails.MailService;
using Soditech.IntelPrev.Users.Application;
using Soditech.IntelPrev.Users.Persistence;
using Soditech.IntelPrev.Users.Persistence.EfCore;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.Persistence.OpenIddict;
using Soditech.IntelPrev.Users.WebApi.Extensions.OpenIddict;

public class UserModuleInitializer : DefaultModuleInitializer
{
    public override void Initialize(WebApplicationBuilder builder, IConfiguration moduleConfiguration)
    {
        base.Initialize(builder, moduleConfiguration);

        builder.Services.AddPersistenceServices(moduleConfiguration);

        builder.Services.AddUserServices();

        builder.AddOpenIddictServices();
        builder.Services.AddOpenIddictServices(builder.Configuration);

        builder.Services.AddScoped<OpenIddictDataSeeder>();
        builder.Services.AddHostedService<OpenIddictDataSeed>();

        builder.Services
            .AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                // the default password length is 6. 

                //TODO: uncomment the following lines to change the password policy
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;

                options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Username;
                options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
                options.ClaimsIdentity.EmailClaimType = OpenIddictConstants.Claims.Email;

            })
            .AddRoles<Role>()
            .AddDefaultUI()
            .AddEntityFrameworkStores<UserDbContext>();

        builder.Services.AddIdentityApiEndpoints<User>();

        builder.Services.AddFastEndpoints();
        
        builder.Services.AddMailServerServices();
        builder.Services.AddHttpContextAccessor();
    }
}