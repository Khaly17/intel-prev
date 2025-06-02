using FastEndpoints;
using FastEndpoints.Swagger;
using Sensor6ty.WebApi;
using Soditech.IntelPrev.Reports.Application;
using Soditech.IntelPrev.Reports.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
// Add services to the container.

builder.Services.SwaggerDocument();

builder.AddModule<ReportModuleInitializer>();
builder.Services.AddOpenIddictServices(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseHttpsRedirection();

//app.MapHub<NotificationsHub>("/notifications");

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();