using System;
using Microsoft.EntityFrameworkCore;
using SpiEyes.Models;

namespace SpiEyes.DAL;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get;set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions) {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=App.db");
    }
}