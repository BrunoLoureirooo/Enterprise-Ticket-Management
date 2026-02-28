using backend.Repository;
using backend.Entities.Models;
using backend.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.WebHost.UseSentry();


// Add Swagger Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add Controllers
builder.Services.AddControllers();

// Add Database Context
builder.Services.AddDbContext<RepositoryContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder
        .WithOrigins("http://localhost:5001", "https://localhost:5001")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Add Identity (use AddIdentityCore to avoid overriding JWT auth schemes)
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddIdentityCore<ApplicationUser>(o =>
{
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 10;
    o.User.RequireUniqueEmail = true;
})
.AddRoles<ApplicationRole>()
.AddEntityFrameworkStores<RepositoryContext>()
.AddDefaultTokenProviders()
.AddSignInManager();


// Add JWT Configuration
builder.Services.Configure<backend.Entities.JwtConfiguration>(builder.Configuration.GetSection("JwtSettings"));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(backend.Application.MappingProfile));

// Add Logger Manager
builder.Services.AddScoped<backend.Application.Services.Contracts.ILoggerManager, backend.Application.Services.LoggerManager>();

// Add Repository Manager
builder.Services.AddScoped<backend.Repository.Contracts.IRepositoryManager, RepositoryManager>();

// Add Service Manager
builder.Services.AddScoped<backend.Application.Services.Contracts.IServiceManager, ServiceManager>();

// Add Validation Filter Attribute
builder.Services.AddScoped<backend.API.ActionFilters.ValidationFilterAttribute>();


var app = builder.Build();

// Automatically apply EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<backend.Repository.RepositoryContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
