using DoctorsClinic.Core.Mapster;
using DoctorsClinic.Infrastructure.Data;
using DoctorsClinic.Infrastructure.IRepositories;
using DoctorsClinic.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  DbContext => Connection String
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register RepositoryWrapper With IRepositoryWrapper
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

// Mapster mappings
MappingConfig.ConfigureMappings();

var app = builder.Build();

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
