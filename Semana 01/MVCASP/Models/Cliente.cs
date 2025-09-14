namespace MVCASP.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Cliente
    {
        public int Id { get; set; }
        [Display(Name ="Nombre de Cliente")]
        [Length(3,100, ErrorMessage ="Ingrese valores")]
        [Required(ErrorMessage ="Campo requerido")]
        public string Nombre_Cliente { get; set; }
        [Required(ErrorMessage = "Campo requerido")]

        public string Apellido_Cliente { get; set; }
        [Required(ErrorMessage = "Campo requerido")]

        public string Direccion_Cliente { get; set; }
        [Required]
        public string Telefono_Cliente { get; set; }
        public DateOnly Fecha_Nacimiento { get; set; }
    }
}
