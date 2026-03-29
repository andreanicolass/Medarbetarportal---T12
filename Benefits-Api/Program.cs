using Benefits_Api.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Hantera JSON-inställningar (viktigt för listor och relationer)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 2. Koppling till databasen
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=benefits.db"));

// 3. CORS - Detta är vad din polare i React behöver!
// Det tillåter hans app att anropa ditt API trots att de körs på olika portar.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddOpenApi();

var app = builder.Build();

// 4. Konfigurera Swagger/Scalar för dokumentation
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Om din polare får problem med "HTTPS/SSL" i React, kan du tillfälligt 
// kommentera ut raden nedan under utveckling.
app.UseHttpsRedirection();

// VIKTIGT: UseCors måste ligga här, efter redirection men före controllers
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();