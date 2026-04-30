using LibraryProxy.API.HealthChecks;
using LibraryProxy.API.Middleware;
using LibraryProxy.Application;
using LibraryProxy.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
                     .ReadFrom.Services(services)
                     .Enrich.FromLogContext());

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new()
        {
            Title = "LibraryProxy",
            Version = "v1",
            Description = ""
        });
    });

    var isDevelopment = builder.Environment.IsDevelopment();
    var allowedOrigins = isDevelopment
        ? builder.Configuration.GetSection("Cors:AllowedOriginsDevelopment").Get<string[]>() ?? []
        : builder.Configuration.GetSection("Cors:AllowedOriginsProduction").Get<string[]>() ?? [];

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    builder.Services.AddHealthChecks()
        .AddCheck<FakeRestApiHealthCheck>("fake-rest-api");

    var app = builder.Build();

    app.UseMiddleware<GlobalExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryProxy API v1");
        });
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowFrontend");
    app.UseSerilogRequestLogging();
    app.UseAuthorization();
    app.MapControllers();
    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
