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
    public DbSet<Solicitud> Solicitudes { get; set; } = null!;

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
            .HasColumnName("CreatedAt")
            .HasColumnType("timestamp without time zone");

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

        modelBuilder.Entity<Usuario>()
            .Property(u => u.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<Protectora>()
            .ToTable("Protectoras")
            .HasKey(p => p.Id);

        modelBuilder.Entity<Protectora>()
            .Property(p => p.UserId)
            .HasColumnName("UserId");

        modelBuilder.Entity<Protectora>()
            .Property(p => p.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("timestamp without time zone");

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

        modelBuilder.Entity<Solicitud>()
            .ToTable("Solicitudes")
            .HasKey(s => s.Id);

        modelBuilder.Entity<Solicitud>()
            .Property(s => s.Id)
            .HasColumnName("id");

        modelBuilder.Entity<Solicitud>()
            .Property(s => s.UsuarioAdoptanteId)
            .HasColumnName("usuario_adoptante_id")
            .IsRequired();

        modelBuilder.Entity<Solicitud>()
            .Property(s => s.UsuarioProtectoraId)
            .HasColumnName("usuario_protectora_id")
            .IsRequired();

        modelBuilder.Entity<Solicitud>()
            .Property(s => s.AnimalId)
            .HasColumnName("animal_id")
            .IsRequired();

        modelBuilder.Entity<Solicitud>()
            .Property(s => s.Estado)
            .HasColumnName("estado")
            .IsRequired()
            .HasMaxLength(15);

        modelBuilder.Entity<Solicitud>()
            .Property(s => s.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("timestamp without time zone")
            .IsRequired();

        modelBuilder.Entity<Solicitud>()
            .Property(s => s.Comentario)
            .HasColumnName("comentario")
            .HasMaxLength(500);

        modelBuilder.Entity<Solicitud>()
            .HasOne(s => s.UsuarioAdoptante)
            .WithMany()
            .HasForeignKey(s => s.UsuarioAdoptanteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Solicitud>()
            .HasOne(s => s.UsuarioProtectora)
            .WithMany()
            .HasForeignKey(s => s.UsuarioProtectoraId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Solicitud>()
            .HasOne(s => s.Animal)
            .WithMany()
            .HasForeignKey(s => s.AnimalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
