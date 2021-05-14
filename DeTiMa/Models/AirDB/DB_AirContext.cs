using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace DeTiMa.Models.AirDB
{
    public class DB_AirContext : DbContext
    {
        public DbSet<AirTicket> AirTicket { get; set; }

        public DB_AirContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@$"Filename={Environment.CurrentDirectory + @"\Air.db"}");
        }
    }
}