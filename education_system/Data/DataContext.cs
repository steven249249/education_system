using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using education_system.Models;

namespace education_system.Data
{
    public class DataContext : DbContext
    {
        //base(option):執行父類別的建構子
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //將所有資料表列過來
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{

        //}
    }
}
