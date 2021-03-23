using App.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<BaseEntity> BaseEntities { get; set; }
    }
}
