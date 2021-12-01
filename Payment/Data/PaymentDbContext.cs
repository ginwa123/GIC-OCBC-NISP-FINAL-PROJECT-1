using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Payment.Models;
using System;

namespace Payment.Data
{
    public class PaymentDbContext : IdentityDbContext
    {
        public virtual DbSet<Models.Payment> Payments { get; set; }
        public virtual DbSet<Models.RefreshToken> RefreshTokens { get; set; }
        public IConfiguration Configuration { get; }

        public PaymentDbContext(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        // postgresql connection 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var pgUserId = Environment.GetEnvironmentVariable("POSTGRES_ID");
            var pgPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            var pgHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            var pgPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            var pgDatabase = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");

            var connStr = $"Server={pgHost};Port={pgPort};User Id={pgUserId};Password={pgPassword};Database={pgDatabase};sslmode=Prefer;Trust Server Certificate=true;";


            optionsBuilder.UseNpgsql(connStr);

        }


    }
}
