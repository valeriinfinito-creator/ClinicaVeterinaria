using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Models
{
    public class Paciente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especie es obligatoria")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string Especie { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Raza { get; set; }

        [Range(0, 30, ErrorMessage = "Edad inválida")]
        public int Edad { get; set; }

        [StringLength(100)]
        public string? Propietario { get; set; }
    }
}