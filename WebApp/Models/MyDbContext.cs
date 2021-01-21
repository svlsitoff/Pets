using Microsoft.EntityFrameworkCore;


namespace WebApp.Models
{
    public class MyDbContext : DbContext
    {
        public DbSet<Shelter> Shelter { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
