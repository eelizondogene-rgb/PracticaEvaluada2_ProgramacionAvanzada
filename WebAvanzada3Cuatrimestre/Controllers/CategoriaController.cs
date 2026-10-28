using Microsoft.AspNetCore.Mvc;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Application.Services;

namespace WebAvanzada3Cuatrimestre.Controllers
{
    // Controlador del modulo de administracion de categorias. Solo coordina las peticiones HTTP,
    // toda la logica de negocio vive en la capa de aplicacion (ICategoriaServicio)
    public class CategoriaController : Controller
    {
        private readonly ICategoriaServicio _categoriaServicio;

        public CategoriaController(ICategoriaServicio categoriaServicio) // Inyección de dependencias
        {
            _categoriaServicio = categoriaServicio;
        }

        /// <summary>
        /// Muestra la pantalla de administración. Los datos de la tabla se cargan por AJAX desde ObtenerCategorias.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene todas las categorias en formato JSON para la tabla.
        /// </summary>
        /// <param name="cancellationToken">Token que se cancela si el cliente aborta la petición.</param>
        /// <returns>Lista de categorias.</returns>
        public async Task<IActionResult> ObtenerCategorias(CancellationToken cancellationToken)
        {
            var categorias = await _categoriaServicio.ObtenerCategoriasAsync(cancellationToken);
            return Json(categorias);
        }

        /// <summary>
        /// Obtiene una categoria por su id en formato JSON para cargar el formulario de edición.
        /// </summary>
        /// <param name="id">Id de la categoria.</param>
        /// <param name="cancellationToken">Token que se cancela si el cliente aborta la petición.</param>
        /// <returns>La categoria encontrada o null.</returns>
        public async Task<IActionResult> ObtenerCategoriaPorId(int id, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaServicio.ObtenerCategoriaPorIdAsync(id, cancellationToken);
            return Json(categoria);
        }

        /// <summary>
        /// Crea una nueva categoria aplicando las reglas de negocio del servicio.
        /// </summary>
        /// <param name="categoria">Datos del formulario (binding model).</param>
        /// <param name="cancellationToken">Token que se cancela si el cliente aborta la petición.</param>
        /// <returns>Objeto Respuesta con el resultado de la operación.</returns>
        [HttpPost]
        public async Task<IActionResult> CrearCategoria(CategoriaDto categoria, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var respuesta = await _categoriaServicio.CrearCategoriaAsync(categoria, cancellationToken);
            return Json(respuesta);
        }

        /// <summary>
        /// Actualiza una categoria existente aplicando las reglas de negocio del servicio.
        /// </summary>
        /// <param name="categoria">Datos del formulario (binding model), incluye el Id.</param>
        /// <param name="cancellationToken">Token que se cancela si el cliente aborta la petición.</param>
        /// <returns>Objeto Respuesta con el resultado de la operación.</returns>
        [HttpPost]
        public async Task<IActionResult> ActualizarCategoria(CategoriaDto categoria, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var respuesta = await _categoriaServicio.ActualizarCategoriaAsync(categoria, cancellationToken);
            return Json(respuesta);
        }

        /// <summary>
        /// Elimina una categoria.
        /// </summary>
        /// <param name="id">Id de la categoria.</param>
        /// <param name="cancellationToken">Token que se cancela si el cliente aborta la petición.</param>
        /// <returns>Objeto Respuesta con el resultado de la operación.</returns>
        [HttpDelete]
        public async Task<IActionResult> EliminarCategoria(int id, CancellationToken cancellationToken)
        {
            var respuesta = await _categoriaServicio.EliminarCategoriaAsync(id, cancellationToken);
            return Json(respuesta);
        }
    }
}
