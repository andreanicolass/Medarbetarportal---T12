using EcobomyApi.Data;
using Microsoft.EntityFrameworkCore;

namespace EcobomyApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<EconomyDbContext>(options =>
            options.UseSqlite("Data Source=economy.db"));

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Swagger endast i development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication(); // 👈 viktigt
        app.UseAuthorization();

        app.MapControllers();

        // Auto-migrate DB
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<EconomyDbContext>();
            db.Database.Migrate();
        }

        app.Run();
    }
}