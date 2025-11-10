namespace AdoptameDAW.Models.DTOs;

public class AnimalDto
{

    public Guid Uuid { get; set; }

    public string Nombre { get; set; } = null!;

    public String Tipo { get; set; }

    public string? Raza { get; set; }

    public int? Edad { get; set; }

    public string? Genero { get; set; }

    public string? Descripcion { get; set; }

    public int ProtectoraId { get; set; }

    public string? ImagenPrincipal { get; set; }

    public DateTime CreatedAt { get; set; }
}
