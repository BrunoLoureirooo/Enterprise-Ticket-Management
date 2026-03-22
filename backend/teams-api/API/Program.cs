using teams.Repository;
using teams.Repository.Contracts;
using teams.Application;
using teams.Application.Services;
using teams.Application.Services.Contracts;
using teams.Application.Messaging;
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
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
    options.AddPolicy("CorsPolicy", policy =>
        policy.WithOrigins("http://localhost:5001", "https://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader()));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddHostedService<UserSyncConsumer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        scope.ServiceProvider.GetRequiredService<RepositoryContext>().Database.Migrate();
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Migration failed");
    }
}

app.UseSwagger();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();
