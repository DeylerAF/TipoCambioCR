﻿using ServerLibrary.Helpers;
using ServerLibrary.Repositories.Contracts;
using ServerLibrary.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Registrar la configuracion para CentralBankService
builder.Services.Configure<CentralBankServiceOptions>(
    builder.Configuration.GetSection("CentralBankService"));

// Registrar los servicios
builder.Services.AddScoped<IExchangeRateInterface, ExchangeRateRepository>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Use the defined CORS policy
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();