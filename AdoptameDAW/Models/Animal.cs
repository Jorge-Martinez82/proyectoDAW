using AdoptameDAW.Models.Enums;

namespace AdoptameDAW.Models;

public partial class Animal
{
    public int Id { get; set; }

    public Guid Uuid { get; set; }

    public string Nombre { get; set; } = null!;

    public TipoAnimal Tipo { get; set; }

    public string? Raza { get; set; }

    public int? Edad { get; set; }

    public string? Genero { get; set; }

    public string? Descripcion { get; set; }

    public int ProtectoraId { get; set; }

    public string? ImagenPrincipal { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Protectora Protectora { get; set; } = null!;
}
