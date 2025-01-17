using Serilog;
using Serilog.Events;
using WebApi.Extensions.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins")!.Split(",");

builder.Services.ConfigureCors(allowedOrigins);
builder.Services.ConfigureRepository();
builder.Services.ConfigureService();
builder.Services.ConfigureDbContext();
builder.Services.ConfigureMapster();
builder.Services.ConfigureBackupService();
builder.Services.ConfigureExceptionHandler();

var app = builder.Build();

await app.InitializeDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "Handled {RequestPath}";
    
    options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;
    
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
    };
});

app.UseExceptionHandler(_ => { });

app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();

app.Run();