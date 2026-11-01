using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Application.Repositories;

namespace WebAvanzada3Cuatrimestre.Application.Services
{
    public class CategoriaServicio : ICategoriaServicio
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly AutoMapper.IMapper _mapper;

        public CategoriaServicio(ICategoriaRepository categoriaRepository, AutoMapper.IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDto>> ObtenerCategoriasAsync(CancellationToken cancellationToken = default)
        {
            var categorias = await _categoriaRepository.GetAllCategoriasAsync(cancellationToken);
            return _mapper.Map<List<CategoriaDto>>(categorias);
        }

        public async Task<CategoriaDto?> ObtenerCategoriaPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var categoria = await _categoriaRepository.GetCategoriaByIdAsync(id, cancellationToken);
            return _mapper.Map<CategoriaDto>(categoria);
        }

        public async Task<Respuesta<CategoriaDto>> CrearCategoriaAsync(CategoriaDto categoria, CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<CategoriaDto>();
            var categoriaEntity = _mapper.Map<Domain.Categoria>(categoria);
            categoriaEntity.Nombre = (categoriaEntity.Nombre ?? string.Empty).Trim();

            if (!categoriaEntity.ValidarReglaNegocioNombreRequerido())
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "El nombre de la categoría es requerido.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (await _categoriaRepository.ExistsCategoriaByNombreAsync(categoriaEntity.Nombre, categoriaEntity.Id, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "Ya existe una categoría con ese nombre.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (!await _categoriaRepository.CreateCategoriaAsync(categoriaEntity, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo crear la categoría.";
                respuesta.Codigo = 500;
                return respuesta;
            }

            respuesta.Dato = _mapper.Map<CategoriaDto>(categoriaEntity);
            return respuesta;
        }

        public async Task<Respuesta<CategoriaDto>> ActualizarCategoriaAsync(CategoriaDto categoria, CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<CategoriaDto>();
            var categoriaEntity = _mapper.Map<Domain.Categoria>(categoria);
            categoriaEntity.Nombre = (categoriaEntity.Nombre ?? string.Empty).Trim();

            if (!categoriaEntity.ValidarReglaNegocioNombreRequerido())
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "El nombre de la categoría es requerido.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (await _categoriaRepository.ExistsCategoriaByNombreAsync(categoriaEntity.Nombre, categoriaEntity.Id, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "Ya existe una categoría con ese nombre.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (!await _categoriaRepository.UpdateCategoriaAsync(categoriaEntity, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo actualizar la categoría.";
                respuesta.Codigo = 500;
                return respuesta;
            }

            respuesta.Dato = _mapper.Map<CategoriaDto>(categoriaEntity);
            return respuesta;
        }

        public async Task<Respuesta<CategoriaDto>> EliminarCategoriaAsync(int id, CancellationToken cancellationToken = default)
        {
            var respuesta = new Respuesta<CategoriaDto>();

            if (await _categoriaRepository.TieneProductosAsociadosAsync(id, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se puede eliminar la categoría porque tiene productos asociados.";
                respuesta.Codigo = 400;
                return respuesta;
            }

            if (!await _categoriaRepository.DeleteCategoriaAsync(id, cancellationToken))
            {
                respuesta.EsCorrecto = false;
                respuesta.Mensaje = "No se pudo eliminar la categoría.";
                respuesta.Codigo = 500;
            }

            return respuesta;
        }
    }
}