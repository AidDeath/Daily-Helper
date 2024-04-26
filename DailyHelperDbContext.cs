using Daily_Helper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daily_Helper
{
    public class DailyHelperDbContext : DbContext
    {
        public DbSet<FailureEvent> FailureEvents => Set<FailureEvent>();
        public DbSet<RoutineIdentifer> RoutineIdentifers => Set<RoutineIdentifer>();
        public DbSet<MailReciever> MailRecievers => Set<MailReciever>();
        public DbSet<Email> Emails => Set<Email>();
        public DbSet<MailLog> MailLogs => Set<MailLog>();



        public DailyHelperDbContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=dailyHelper.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}
