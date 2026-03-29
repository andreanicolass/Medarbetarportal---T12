using Microsoft.EntityFrameworkCore;
using ApplicationsApi.Models;

namespace ApplicationsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<BenefitApplication> Applications { get; set; }
}