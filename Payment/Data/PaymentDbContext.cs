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

            // hardcoded ? cara biar dapet environmentvariable dari heroku bagaimana ???
            if (pgUserId == null) pgUserId = "xczrclkqhpjdvq";
            if (pgPassword == null) pgPassword = "a3bd88fcdc51dbd926ae76e80f98d77331cfe23fb8563b0b2c579d890d76d37c";
            if (pgHost == null) pgHost = "ec2-54-146-116-84.compute-1.amazonaws.com";
            if (pgPort == null) pgPort = "5432";
            if (pgDatabase == null) pgDatabase = "d4b1reod9a97fh";
            var connStr = $"Server={pgHost};Port={pgPort};User Id={pgUserId};Password={pgPassword};Database={pgDatabase};sslmode=Prefer;Trust Server Certificate=true;";


            optionsBuilder.UseNpgsql(connStr);

        }


    }
}
