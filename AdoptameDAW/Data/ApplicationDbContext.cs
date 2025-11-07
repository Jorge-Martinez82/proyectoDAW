using AdoptameDAW.Models;
using AdoptameDAW.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AdoptameDAW.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Animal> Animales { get; set; } = null!;
    public DbSet<Protectora> Protectoras { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar tabla Animal
        modelBuilder.Entity<Animal>()
            .ToTable("Animales")
            .HasKey(a => a.Id);

        // mapea el tipo de animal de enum a string y viceversa
        modelBuilder.Entity<Animal>()
            .Property(a => a.Tipo)
            .HasColumnName("Tipo")
            .HasConversion(
                v => v.ToString(),  
                v => Enum.Parse<TipoAnimal>(v)  
            );

        modelBuilder.Entity<Animal>()
            .Property(a => a.ProtectoraId).HasColumnName("ProtectoraId");

        modelBuilder.Entity<Animal>()
            .Property(a => a.ImagenPrincipal).HasColumnName("ImagenPrincipal");

        modelBuilder.Entity<Animal>()
            .Property(a => a.CreatedAt).HasColumnName("CreatedAt");

        // relacion animal-protectora
        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Protectora)
            .WithMany(p => p.Animales)
            .HasForeignKey(a => a.ProtectoraId);

        // configura tabla usuario
        modelBuilder.Entity<Usuario>()
            .ToTable("Usuarios")
            .HasKey(u => u.Id);

        // mapea el string de BD a enum tipoususario
        modelBuilder.Entity<Usuario>()
            .Property(u => u.TipoUsuario)
            .HasColumnName("TipoUsuario")
            .HasConversion(
                v => v.ToString(),  
                v => Enum.Parse<TipoUsuario>(v)  
            );

        // configura tabla protectoras
        modelBuilder.Entity<Protectora>()
            .ToTable("Protectoras")
            .HasKey(p => p.Id);

        modelBuilder.Entity<Protectora>()
            .Property(p => p.UserId).HasColumnName("UserId");

        // realacion protectora/usuario
        modelBuilder.Entity<Protectora>()
            .HasOne(p => p.User)
            .WithMany(u => u.Protectoras)
            .HasForeignKey(p => p.UserId);
    }
}
