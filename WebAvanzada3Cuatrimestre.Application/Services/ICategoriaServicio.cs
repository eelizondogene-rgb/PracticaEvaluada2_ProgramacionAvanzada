using System;
using System.Collections.Generic;
using System.Text;
using WebAvanzada3Cuatrimestre.Application.Dtos;

namespace WebAvanzada3Cuatrimestre.Application.Services
{
    public interface ICategoriaServicio
    {
        Task<List<CategoriaDto>> ObtenerCategoriasAsync(CancellationToken cancellationToken = default);
        Task<CategoriaDto?> ObtenerCategoriaPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Respuesta<CategoriaDto>> CrearCategoriaAsync(CategoriaDto categoria, CancellationToken cancellationToken = default);
        Task<Respuesta<CategoriaDto>> ActualizarCategoriaAsync(CategoriaDto categoria, CancellationToken cancellationToken = default);
        Task<Respuesta<CategoriaDto>> EliminarCategoriaAsync(int id, CancellationToken cancellationToken = default);
    }
}