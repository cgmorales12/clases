using System.ComponentModel.DataAnnotations;

namespace MVCASP.Models
{
    public class Provincia
    {
        public int Id { get; set; }
        [Display(Name ="Estado / Provincia")]
        public string Nombre_Provincia { get; set; }

        /// <summary>
        /// //////
        /// </summary>
        public int PaisId { get; set; }
        public Pais Pais { get; set; }
    }
}
