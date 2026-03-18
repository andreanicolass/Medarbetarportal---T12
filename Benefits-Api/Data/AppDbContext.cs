using Benefits_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Benefits_Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Benefit> Benefits { get; set; }
    public DbSet<Category> Categories { get; set; }
}