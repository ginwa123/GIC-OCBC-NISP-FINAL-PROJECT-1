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
        private static bool IsDevelopment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        // postgresql connection 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr;
            var pgUserId = Environment.GetEnvironmentVariable("POSTGRES_ID");
            var pgPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            var pgHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            var pgPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            var pgDatabase = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");

            connStr = $"Server={pgHost};Port={pgPort};User Id={pgUserId};Password={pgPassword};Database={pgDatabase};sslmode=Prefer;Trust Server Certificate=true;";
            if (!IsDevelopment) connStr = GetHerokuConnectionString();




            optionsBuilder.UseNpgsql(connStr);

        }

        private string GetHerokuConnectionString()
        {
            // Get the connection string from the ENV variables
            string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // parse the connection string
            var databaseUri = new Uri(connectionUrl);

            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

            return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
        }


    }
}
