using Microsoft.AspNetCore.Mvc;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Application.Services;

namespace WebAvanzada3Cuatrimestre.Controllers;

public class ProductoController : Controller
{
    private readonly IProductoServicio _productoServicio;
    private readonly ICategoriaServicio _categoriaServicio;

    public ProductoController(IProductoServicio productoServicio, ICategoriaServicio categoriaServicio)
    {
        _productoServicio = productoServicio;
        _categoriaServicio = categoriaServicio;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Categorias = await _categoriaServicio.ObtenerCategoriasAsync(cancellationToken);
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerProductos(CancellationToken cancellationToken)
    {
        return Json(await _productoServicio.ObtenerProductosAsync(cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerProductoPorId(int id, CancellationToken cancellationToken)
    {
        var producto = await _productoServicio.ObtenerProductoPorIdAsync(id, cancellationToken);
        return producto is null ? NotFound() : Json(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearProducto(ProductoDto producto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Json(await _productoServicio.CrearProductoAsync(producto, cancellationToken));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActualizarProducto(ProductoDto producto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Json(await _productoServicio.ActualizarProductoAsync(producto, cancellationToken));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarProducto(int id, CancellationToken cancellationToken)
    {
        return Json(await _productoServicio.EliminarProductoAsync(id, cancellationToken));
    }
}
