using AdoptameDAW.Models;
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
    public DbSet<Adoptante> Adoptantes { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Animal>()
            .ToTable("Animales")
            .HasKey(a => a.Id);

        modelBuilder.Entity<Animal>()
            .Property(a => a.Tipo)
            .HasColumnName("Tipo")
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Animal>()
            .Property(a => a.ProtectoraId)
            .HasColumnName("ProtectoraId");

        modelBuilder.Entity<Animal>()
            .Property(a => a.ImagenPrincipal)
            .HasColumnName("ImagenPrincipal");

        modelBuilder.Entity<Animal>()
            .Property(a => a.CreatedAt)
            .HasColumnName("CreatedAt");

        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Protectora)
            .WithMany(p => p.Animales)
            .HasForeignKey(a => a.ProtectoraId);

        modelBuilder.Entity<Usuario>()
            .ToTable("Usuarios")
            .HasKey(u => u.Id);

        modelBuilder.Entity<Usuario>()
            .Property(u => u.TipoUsuario)
            .HasColumnName("TipoUsuario")
            .IsRequired()
            .HasMaxLength(50); 

        modelBuilder.Entity<Protectora>()
            .ToTable("Protectoras")
            .HasKey(p => p.Id);

        modelBuilder.Entity<Protectora>()
            .Property(p => p.UserId)
            .HasColumnName("UserId");

        modelBuilder.Entity<Protectora>()
            .HasOne(p => p.User)
            .WithMany(u => u.Protectoras)
            .HasForeignKey(p => p.UserId);
        modelBuilder.Entity<Adoptante>()
            .ToTable("Adoptantes")
            .HasKey(a => a.Id);

        modelBuilder.Entity<Adoptante>()
            .Property(a => a.UserId)
            .HasColumnName("UserId");

        modelBuilder.Entity<Adoptante>()
            .HasOne(a => a.Usuario)
            .WithMany(u => u.Adoptantes)
            .HasForeignKey(a => a.UserId);

    }
}
