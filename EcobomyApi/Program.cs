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

        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        
        builder.Services.AddOpenApi();

        var app = builder.Build();

        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication(); 
        app.UseAuthorization();

        app.MapControllers();

        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<EconomyDbContext>();
            db.Database.Migrate();
        }

        app.Run();
    }
}