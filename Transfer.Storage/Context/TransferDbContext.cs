using Microsoft.EntityFrameworkCore;
using Transfer.Application.Orchestrators;

namespace Transfer.Storage
{
    public class TransferDbContext : DbContext
    {
        public TransferDbContext(DbContextOptions<TransferDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransferStateEntityConfiguration());
        }

        public DbSet<TransferState> TransferStates { get; set; }
    }
}
