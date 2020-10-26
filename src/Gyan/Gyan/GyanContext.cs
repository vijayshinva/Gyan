using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Gyan
{
    class GyanContext : DbContext
    {
        private readonly ILogger logger;
        private readonly IConfigurationRoot configuration;

        public GyanContext(ILogger logger, IConfigurationRoot configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        private string GetConnectionString(string name = "GyanDB")
        {
            return configuration.GetConnectionString(name);
        }

        public DbSet<Article> Articles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(GetConnectionString());
        }
    }
}
