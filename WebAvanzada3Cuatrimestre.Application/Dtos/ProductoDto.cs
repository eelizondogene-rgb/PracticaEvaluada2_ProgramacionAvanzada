using System.ComponentModel.DataAnnotations;

namespace WebAvanzada3Cuatrimestre.Application.Dtos;

public class ProductoDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría.")]
    public int CategoriaId { get; set; }

    public string CategoriaNombre { get; set; } = string.Empty;
}
