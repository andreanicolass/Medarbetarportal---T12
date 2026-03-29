using Microsoft.EntityFrameworkCore;
using StatistikAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrera DbContext med SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=reports.db"));

builder.Services.AddControllers();
builder.Services.AddHttpClient(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===============================
// ✅ ENDA TILLÄGGET: CORS
// ===============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Swagger i development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ⚠️ BEHÅLLER DIN KOD
app.UseStaticFiles();

app.MapGet("/", () => Results.Redirect("/index.html"));

// ===============================
// ✅ ENDA MIDDLEWARE TILLÄGG
// ===============================
app.UseCors("AllowReact");

app.UseAuthorization();
app.MapControllers();

app.Run();