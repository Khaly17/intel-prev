using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sensor6ty.WebApi;
using Soditech.IntelPrev.Mediatheques.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddModule<MediathequeModuleInitializer>();
builder.Services.AddOpenIddictServices(builder.Configuration);
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    //app.UseSwaggerGen();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseFastEndpoints();

app.Run();