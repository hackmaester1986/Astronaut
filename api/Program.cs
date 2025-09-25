using Microsoft.EntityFrameworkCore;
using Stargate.Repositories;
using Stargate.Repositories.Impl;
using Stargate.Services;
using Stargate.Services.Impl;
using StargateApi.Repositories;
using StargateAPI.Business.Data;
using StargateAPI.Repositories.Impl;
using StargateAPI.Services;
using StargateAPI.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StargateContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("StarbaseApiDatabase")));


builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IAstronautDutyRepository, AstronautDutyRepository>();
builder.Services.AddScoped<IAstronautDetailRepository, AstronautDetailRepository>();
builder.Services.AddScoped<IProcessLogRepository, ProcessLogRepository>();

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IAstronautDutyService, AstronautDutyService>();
builder.Services.AddScoped<IAstronautDetailService, AstronautDetailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();


