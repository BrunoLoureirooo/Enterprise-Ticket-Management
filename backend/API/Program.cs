using backend.Repository;
using backend.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

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
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Add Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
{
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 10;
    o.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<RepositoryContext>()
.AddDefaultTokenProviders();


// Add JWT Configuration
builder.Services.Configure<backend.Entities.JwtConfiguration>(builder.Configuration.GetSection("JwtSettings"));

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(backend.Application.MappingProfile));

// Add Logger Manager
builder.Services.AddScoped<backend.Service.Contracts.ILoggerManager, backend.Service.LoggerManager>();

// Add Repository Manager
builder.Services.AddScoped<backend.Repository.Contracts.IRepositoryManager, RepositoryManager>();

// Add Service Manager
builder.Services.AddScoped<backend.Service.Contracts.IServiceManager, backend.Service.ServiceManager>();

// Add Validation Filter Attribute
builder.Services.AddScoped<backend.API.ActionFilters.ValidationFilterAttribute>();


// Add Authentication
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
        ValidAudience = builder.Configuration["JwtSettings:ValidAudience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(System.Environment.GetEnvironmentVariable("SECRET") ?? "This is a failing default secret just in case"))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
