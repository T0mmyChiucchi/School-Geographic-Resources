using Microsoft.EntityFrameworkCore;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Infrastructure.Persistence;
using SchoolGeoResources.Application.Organizations.Commands.CreateOrganization;
using System.Reflection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SchoolGeoResources.Application.Common.Interfaces;
using SchoolGeoResources.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Supabase:Authority"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Supabase:Authority"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Supabase:ValidAudience"],
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddControllers();

// Configure OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext (using in-memory for now if connection string isn't set, otherwise PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    // Fallback to in-memory for testing purposes if not configured
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("SchoolGeoResourcesDb"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString, o => o.UseNetTopologySuite()));
}

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

// Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(SchoolGeoResources.Infrastructure.Persistence.Repositories.Repository<>));
builder.Services.AddScoped<IOrganizationRepository, SchoolGeoResources.Infrastructure.Persistence.Repositories.OrganizationRepository>();
builder.Services.AddScoped<IPlaceRepository, SchoolGeoResources.Infrastructure.Persistence.Repositories.PlaceRepository>();

// Register ApplicationDbContext Interface
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<SchoolGeoResources.Infrastructure.Persistence.ApplicationDbContext>());

// Configure MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrganizationCommand).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
