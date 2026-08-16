


using BlazorBiblioteca.shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorBiblioteca.Data
{
    public class LibrosDBContext : DbContext
    {

        public LibrosDBContext(DbContextOptions<LibrosDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Libro>().ToTable("libros");

        }

        public DbSet<Libro> Libro { get; set; }

    }
}
