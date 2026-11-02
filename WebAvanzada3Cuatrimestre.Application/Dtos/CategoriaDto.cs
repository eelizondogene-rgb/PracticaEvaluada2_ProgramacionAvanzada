using System.ComponentModel.DataAnnotations;

namespace WebAvanzada3Cuatrimestre.Application.Dtos
{
    public class CategoriaDto
    {
        [Required(ErrorMessage = "El identificador de la categoría es obligatorio.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;
    }
}