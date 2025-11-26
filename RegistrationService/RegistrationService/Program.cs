using Microsoft.EntityFrameworkCore;
using RegistrationService.Data;
using RegistrationService.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        .Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST"))
        .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT"))
        .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME"))
        .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER"))
        .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD"));

    options.UseNpgsql(connectionString);
});
builder.Services.AddScoped<IUserRepository, UserRepository>(); 
//builder.Services.AddScoped<IUserRepository, MockUserRepository>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // HTTP
    options.ListenAnyIP(8081); // HTTPS
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers(); 

app.Run();
