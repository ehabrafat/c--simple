using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Conteact_list_exercise.DBAccess
{
    public class ContactDbContext : DbContext
    {
        public DbSet<Person> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactDbContext).Assembly);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var configStr = config["ConnectionStrings:DefaultConnection"];
            optionsBuilder.UseSqlServer(configStr);
        }
    }
}
