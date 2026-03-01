using Serilog;
using gateway_api.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.WebHost.UseSentry();


// Add YARP services and load the configuration from appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder
        .WithOrigins("http://localhost:4200", "https://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddHttpClient("pdp", client => { })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });
builder.Services.AddMemoryCache();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CorsPolicy");

// Swagger UI — aggregates docs from all services via YARP passthrough routes
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/identity/v1/swagger.json", "Identity API");
    c.RoutePrefix = "swagger";
});

app.UseMiddleware<GatewayAuthorizationMiddleware>();

// Map the reverse proxy endpoints
app.MapReverseProxy();

app.Run();
