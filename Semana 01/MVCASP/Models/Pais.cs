namespace MVCASP.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Pais
    {
   
        public int Id { get; set; }
        [Required(ErrorMessage ="Campo Requerido")]
        [Length(3,100,ErrorMessage ="El numero de caracteres es 3 hasta 100")]
        [Display(Name ="Nombre del País")]
        public string Nombre_Pais { get; set; }
    }
}
