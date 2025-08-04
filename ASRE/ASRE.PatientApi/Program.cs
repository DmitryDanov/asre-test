using ASRE.DataLayer.Context;
using ASRE.PatientApi;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"),
        sqlServerOptions => sqlServerOptions.MigrationsAssembly("ASRE.PatientApi"))
    );
builder.Services.AddAutoMapper(config => config.AddMaps(Assembly.GetExecutingAssembly()));
builder.Services.RegisterServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Create a DB if doesn't exist and apply migrations.
using IServiceScope scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync().ConfigureAwait(false);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
