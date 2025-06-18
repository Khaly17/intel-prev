using FastEndpoints;
using FastEndpoints.Swagger;
using Sensor6ty.WebApi;
using Soditech.IntelPrev.Mediatheques.WebApi;
using Soditech.IntelPrev.NotificationHubs.WebApi;
using Soditech.IntelPrev.Preventions.WebApi;
using Soditech.IntelPrev.Reports.Application;
using Soditech.IntelPrev.Reports.WebApi;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.SwaggerDocument(opt =>
{
    opt.ShortSchemaNames = false;
});

builder.Services.AddCors(options =>
{
    var conf = builder.Configuration.GetSection("App");
    options.AddDefaultPolicy(
        policy => policy
            .WithOrigins(conf["CorsOrigins"]!.Split(";"))
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.AddModule<UserModuleInitializer>();
builder.AddModule<MediathequeModuleInitializer>();
builder.AddModule<PreventionModuleInitializer>();
builder.AddModule<ReportModuleInitializer>();
builder.AddModule<NotificationModuleInitializer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}


app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.MapGroup("/api/account/")
    .WithTags("account")
    .MapIdentityApi<User>();

app.MapHub<NotificationsHub>("/notifications");

app.Run();