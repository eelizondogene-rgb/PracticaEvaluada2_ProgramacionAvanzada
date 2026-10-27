using Microsoft.EntityFrameworkCore;
using WebAvanzada3Cuatrimestre.Domain;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
        });
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            entity.HasOne(x => x.Categoria)
                  .WithMany(x => x.Productos)
                  .HasForeignKey(x => x.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
