using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoloNorte_WEB.Models
{
    public class Cliente
    {
        [Required(ErrorMessage = "Este dato es obligatorio")]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "Verifique la longitud de la cedula. (debe ser igual a 12)")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "Este dato es obligatorio")]
        [StringLength(60, MinimumLength = 10, ErrorMessage = "Verifique la longitud del nombre. (MIN: 10 | MAX: 60)")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "Este dato es obligatorio")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "Verifique la longitud del numero telefonico. (Debe ser igual a 9)")]
        public string Telefono { get; set; }


        public bool Descuento { get; set; }


        [Required(ErrorMessage = "Este dato es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Ingrese un numero mayor que 0.")]
        public int ComprasRecientes { get; set; }
        
        
        [Required(ErrorMessage = "Este dato es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Ingrese un numero mayor que 0.")]
        public int ComprasUltimoAño { get; set; }
        
        
        [Required(ErrorMessage = "Este dato es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Ingrese un numero mayor que 0.")]
        public int ComprasAC { get; set; }

    }
}
