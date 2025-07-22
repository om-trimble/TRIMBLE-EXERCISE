using Microsoft.EntityFrameworkCore;
using SupportPortal.Api;
using SupportPortal.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// 👇 Define CORS policy
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // frontend URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

// Add Entity Framework DbContext
builder.Services.AddDbContext<SupportTicketDbContext>(options =>
    options.UseSqlite("Data Source=supporttickets.db"));

var app = builder.Build();

// Ensure database is created and seed with sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SupportTicketDbContext>();
    context.Database.EnsureCreated();
    
    // Seed database with sample tickets
    await DatabaseSeeder.SeedDatabase(context);
}

// 👇 Enable Swagger
    app.UseSwagger();
    app.UseSwaggerUI();

// 👇 Enable CORS before anything else
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

// Make Program class public for integration testing
public partial class Program { }
