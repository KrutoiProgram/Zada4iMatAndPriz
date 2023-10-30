using ApiJwtDemo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);

var app = builder.Build();

UsePipeline(app);

app.Run();


void ConfigureServices(WebApplicationBuilder builder)
{
    string connectionString = builder.Configuration.GetConnectionString("local") ??
        throw new InvalidOperationException("Local connection string not defined");

    builder.Services.AddControllers();
    builder.Services.AddDbContext<UsersContext>(opt => opt.UseSqlite(connectionString));
}

void UsePipeline(WebApplication app)
{
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}