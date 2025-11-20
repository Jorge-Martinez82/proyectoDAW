namespace AdoptameDAW.Models
{
    public class Adoptante
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Poblacion { get; set; }
        public string Provincia { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Usuario Usuario { get; set; }
    }

}
