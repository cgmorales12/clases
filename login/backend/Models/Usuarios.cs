namespace backend.Models
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string email { get; set; }
        public string contrasenia { get; set; }
        public bool activo { get; set; }
    }
}
