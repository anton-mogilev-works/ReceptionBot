using Microsoft.EntityFrameworkCore;
using ReceptionBot.Models;

namespace ReceptionBot.Services
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Button> Buttons => Set<Button>();
        public DbSet<Setting> Settings => Set<Setting>();
        public DbSet<JournalRecord> Journal => Set<JournalRecord>();
        private string DbPath = "database.sqlite3";
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    }
}