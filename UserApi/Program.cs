using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using UserApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// DB 
var dbPath = builder.Environment.IsDevelopment()
    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "user.db")
    : Path.Combine("user.db");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AUTH 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/auth/login";
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

//  CORS 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .SetIsOriginAllowed(origin => origin == "medarbetarportal-ajgsfkg4gug3bpbb.polandcentral-01.azurewebsites.net")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// SEED DATA 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Roles.Any())
    {
        db.Roles.AddRange(
            new Role { Name = "Admin" },
            new Role { Name = "Employee" },
            new Role { Name = "HR" },
            new Role { Name = "FinancialController" },
            new Role { Name = "SystemAdmin" }
        );
    }

    if (!db.Departments.Any())
    {
        db.Departments.AddRange(
            new Department { Name = "Management" },
            new Department { Name = "Staff" },
            new Department { Name = "HR" },
            new Department { Name = "Finance" },
            new Department { Name = "IT" }
        );
    }

    db.SaveChanges();

    void AddUser(string email, string roleName, string depName, string firstName, string lastName, string empId, string password)
    {
        if (!db.Users.Any(u => u.Email == email))
        {
            var role = db.Roles.First(r => r.Name == roleName);
            var dep = db.Departments.First(d => d.Name == depName);

            db.Users.Add(new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                EmployeeId = empId,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                IsApproved = true,
                RoleId = role.Id,
                DepartmentId = dep.Id
            });
        }
    }

    AddUser("admin@gmail.com", "Admin", "Management", "Admin", "User", "0001", "Admin123");
    AddUser("hr@gmail.com", "HR", "HR", "HR", "User", "0002", "Hr12345");
    AddUser("staff@gmail.com", "Employee", "Staff", "Staff", "User", "0003", "Staff123");
    AddUser("sysadmin@gmail.com", "SystemAdmin", "IT", "System", "Admin", "0004", "Admin123");
    AddUser("finance@gmail.com", "FinancialController", "Finance", "Finance", "User", "0005", "Finance123");

    db.SaveChanges();
}

app.Run();