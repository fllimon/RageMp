using Microsoft.EntityFrameworkCore;
using RageMpServer.DatabaseEntities;

namespace RageMpServer.Contexts
{
    class RageContext : DbContext
    {
        private string _connectionString = "server=localhost;port=3306;UserId=root;Password=11111111;database=rageDb;";

        public DbSet<User> Users { get; set; }

        public DbSet<CustomPlayer> Players { get; set; }

        public DbSet<PlayerPosition> PlayersPositions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
        }
    }
}
