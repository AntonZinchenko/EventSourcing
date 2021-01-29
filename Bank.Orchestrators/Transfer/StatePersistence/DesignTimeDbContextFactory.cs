using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SeedWorks;
using System.IO;

namespace Bank.Orchestrators.Transfer.StatePersistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TransferDbContext>
    {
        public TransferDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Bank.Api"))
                .AddJsonFile("appsettings.json")
                .Build();

            return new DbContextOptionsBuilder<TransferDbContext>()
                .Do(builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .PipeTo(builder => new TransferDbContext(builder.Options));
        }
    }
}
