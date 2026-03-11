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

var app = builder.Build();

// Swagger i development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();