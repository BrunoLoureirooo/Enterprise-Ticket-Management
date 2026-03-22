using ticket.Repository;
using ticket.Repository.Contracts;
using ticket.Application;
using ticket.Application.Services.Contracts;
using ticket.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.WebHost.UseSentry();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<RepositoryContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy
        .WithOrigins("http://localhost:5001", "https://localhost:5001")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddHostedService<TeamMembershipConsumer>();
builder.Services.AddHostedService<UserSyncConsumer>();
builder.Services.AddHostedService<TeamSyncConsumer>();
builder.Services.AddHostedService<ProjectSyncConsumer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<RepositoryContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.UseSwagger();

app.UseCors("CorsPolicy");

app.MapControllers();

app.MapGet("/health/sentry-test", () =>
{
    SentrySdk.CaptureMessage("Hello Sentry");
    return Results.Ok("Sentry test message sent.");
});

app.Run();
