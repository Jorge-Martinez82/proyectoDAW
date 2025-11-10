namespace AdoptameDAW.Models;

public partial class Protectora
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Provincia { get; set; }

    public string? Email { get; set; }

    public string? Imagen { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Usuario User { get; set; } = null!;

    public ICollection<Animal> Animales { get; set; } = new List<Animal>();
}
