using System;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sensor6ty.WebApi;
using Soditech.IntelPrev.Users.Persistence.Models;
using Soditech.IntelPrev.Users.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    var conf = builder.Configuration.GetSection("App");
    
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(conf["CorsOrigins"]!.Split(";", StringSplitOptions.RemoveEmptyEntries))
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.SwaggerDocument();

builder.AddModule<UserModuleInitializer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication()
    .UseAuthorization();

app.UseFastEndpoints();

app.MapGroup("/api/account/")
    .WithTags("account")
    .MapIdentityApi<User>();

app.Run();