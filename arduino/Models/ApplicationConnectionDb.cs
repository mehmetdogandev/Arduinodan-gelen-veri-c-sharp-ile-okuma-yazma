using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arduino.Models
{
    internal class ApplicationConnectionDb: DbContext
    {
        public DbSet<Personel> Personel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseMySql("server=localhost;port=3306;user=root;password=;database=arduino")
                .UseLoggerFactory(LoggerFactory.Create(b => b
                .AddFilter(level => level >= LogLevel.Information)))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }

    }
}
