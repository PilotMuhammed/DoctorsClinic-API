using Core.Services;
using DoctorClinic.Api.Helper;
using DoctorClinic.Api.Middleware;
using DoctorsClinic.Api.Middlewares;
using DoctorsClinic.Core.Extensions;
using DoctorsClinic.Core.Mapster;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;
using DoctorsClinic.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScopedServices();
builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
builder.Services.AddScoped<JWTGenerate>();


//  DbContext => Connection String
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Configure CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Mapster
MappingConfig.ConfigureMappings();

// Register RepositoryWrapper With IRepositoryWrapper
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();


// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(option =>
{
    //option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    //option.SwaggerDoc("Test", new OpenApiInfo { Title = "Test ", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    option.SupportNonNullableReferenceTypes();
});

var app = builder.Build();

//app.UseSwaggerUI(options =>
//{
//    options.InjectStylesheet("/swagger/custom.css");
//    options.DefaultModelsExpandDepth(0);
//    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
//    options.SwaggerEndpoint("/swagger/Test/swagger.json", "Test");
//});

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
app.UseSwaggerUI();
}


app.UseRouting();
app.UseCors("Cors");
app.UseHttpsRedirection();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Apply pending migrations and seed initial data
app.MigrateDatabase<AppDbContext>((context, services) =>
{
    var logger = services.GetService<ILogger<DbSeed>>();
    DbSeed.SeedAsync(context, logger!).Wait();
});

app.Run();
