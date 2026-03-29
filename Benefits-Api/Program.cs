using Benefits_Api.Data;
using Benefits_Api.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=benefits.db"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    
    context.Database.Migrate();

    if (!context.Benefits.Any())
    {
        context.Benefits.AddRange(
            new Benefit { 
                Title = "Padel-serien", 
                Description = "Boka banor på lokala center. Friskvårdsbidraget kan användas direkt i appen.", 
                Price = 250, 
                IsActive = true 
            },
            new Benefit { 
                Title = "Golf-medlemskap", 
                Description = "Rabatterade greenfees och tillgång till övningsområden hela säsongen.", 
                Price = 400, 
                IsActive = true 
            },
            new Benefit { 
                Title = "Skidresor & Skipass", 
                Description = "Förmånliga priser på liftkort och boende i både svenska och norska fjällen.", 
                Price = 350, 
                IsActive = true 
            },
            new Benefit { 
                Title = "Gym-medlemskap", 
                Description = "Skaffa medlemskap till ett förmånligt pris", 
                Price = 350, 
                IsActive = true 
            },
            new Benefit { 
                Title = "Företagsmassage", 
                Description = "30 minuter klassisk massage per månad på kontoret.", 
                Price = 350, 
                IsActive = true 
            },
            new Benefit { 
                Title = "Västtrafik Periodkort", 
                Description = "Subventionerat månadskort för zon A+B.", 
                Price = 800, 
                IsActive = true 
            }
        );
        context.SaveChanges();
    }
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();