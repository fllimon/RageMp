using Microsoft.EntityFrameworkCore;
using RageMpServer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageMpServer.Contexts
{
    class RageContext : DbContext
    {
        private string _connectionString = "server=localhost;port=3306;UserId=root;Password=11111111;database=rageDb;";

        public DbSet<User> Users { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerPosition> PlayersPositions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
        }
    }
}
