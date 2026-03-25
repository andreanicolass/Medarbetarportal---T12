using ApplicationsApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient("UserApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7001/");
});

builder.Services.AddHttpClient("BenefitApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7002/");
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=application.db"));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();