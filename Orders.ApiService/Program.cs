using Microsoft.AspNetCore.Diagnostics;
using Orders.ApiService.Examples;
using Orders.ApiService.ServiceInstallers;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Reflection;
using System.Text.Json;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.InstallServices();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.ExampleFilters();
            options.SchemaFilter<OrderStatusSchemaFilter>();
            options.SchemaFilter<CustomerSegmentSchemaFilter>();
        });
        builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
        builder.Services.AddMemoryCache(); // Register IMemoryCache

        var app = builder.Build();

        // Global error handling middleware with structured logging
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("GlobalExceptionHandler");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var error = exceptionHandlerPathFeature?.Error;

                // Log the error with structured logging
                if (error != null)
                {
                    logger.LogError(error, "Unhandled exception occurred while processing request for {Path}", context.Request.Path);
                }
                else
                {
                    logger.LogError("Unhandled exception occurred but no exception details were found for {Path}", context.Request.Path);
                }

                var response = new
                {
                    message = "An unexpected error occurred.",
                    detail = error?.Message,
                    path = context.Request.Path
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            });
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}