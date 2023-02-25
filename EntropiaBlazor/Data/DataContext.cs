using Microsoft.EntityFrameworkCore;
using Models;

namespace EntropiaBlazor.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fuente>().HasData(
                new Fuente { CadenaFuente = "Esta es una fuente ya en memoria" },
                new Fuente { CadenaFuente = "Este texto representa el libro del quijote de la mancha" }
            );
        }
        public DbSet<Fuente> Fuentes => Set<Fuente>();
    }
}
