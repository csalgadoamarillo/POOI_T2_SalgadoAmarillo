using System.ComponentModel.DataAnnotations;

namespace POOI_T2_SalgadoAmarillo.Models
{
    public class Alumno
    {
        [Required(ErrorMessage = "Ingrese el DNI.")]
        [Display(Name = "DNI")]
        public string dni { get; set; }

        [Required(ErrorMessage = "Ingrese los nombres.")]
        [Display(Name = "Nombres")]
        public string nombres { get; set; }

        [Required(ErrorMessage = "Ingrese los apellidos.")]
        [Display(Name = "Apellidos")]
        public string apellidos { get; set; }

        [Required(ErrorMessage = "Ingrese la carrera.")]
        [Display(Name = "Carrera")]
        public string carrera { get; set; }

        [Display(Name = "Ciclo")]
        public int ciclo { get; set; }

        public Alumno()
        {
        }

        public Alumno(string dni, string nombres, string apellidos, string carrera, int ciclo)
        {
            this.dni = dni;
            this.nombres = nombres;
            this.apellidos = apellidos;
            this.carrera = carrera;
            this.ciclo = ciclo;
        }
    }
}
