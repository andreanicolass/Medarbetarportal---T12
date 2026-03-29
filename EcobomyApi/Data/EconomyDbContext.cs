using Microsoft.EntityFrameworkCore;
using EcobomyApi.Models;

namespace EcobomyApi.Data
{
    public class EconomyDbContext : DbContext
    {
        public EconomyDbContext(DbContextOptions<EconomyDbContext> options)
            : base(options)
        {
        }

        public DbSet<EconomyRecord> EconomyRecords { get; set; }
    }
}