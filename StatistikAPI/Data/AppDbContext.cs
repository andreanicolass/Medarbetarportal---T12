using Microsoft.EntityFrameworkCore;
using StatistikAPI.Models;

namespace StatistikAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
    }
}