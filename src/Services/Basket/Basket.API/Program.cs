using Basket.API.Configuration;
using BuildingBlocks.Messaging.MassTransit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

builder.Services
    .AddApplicationServices(assembly)
    .AddDataServices(builder.Configuration)
    .AddGrpcServices(builder.Configuration)
    .AddMessageBroker(builder.Configuration)
    .AddCrossCuttingServices(builder.Configuration);

var app = builder.Build();
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

await app.RunAsync();