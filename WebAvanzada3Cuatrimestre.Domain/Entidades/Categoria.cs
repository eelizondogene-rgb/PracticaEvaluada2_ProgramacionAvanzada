using System.Collections.Generic;

namespace WebAvanzada3Cuatrimestre.Domain;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
