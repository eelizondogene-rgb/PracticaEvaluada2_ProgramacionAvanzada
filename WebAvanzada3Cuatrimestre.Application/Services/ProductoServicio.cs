using AutoMapper;
using WebAvanzada3Cuatrimestre.Application.Dtos;
using WebAvanzada3Cuatrimestre.Application.Repositories;
using WebAvanzada3Cuatrimestre.Domain;

namespace WebAvanzada3Cuatrimestre.Application.Services;

public class ProductoServicio : IProductoServicio
{
    private readonly IProductoRepository _productoRepository;
    private readonly IMapper _mapper;

    public ProductoServicio(IProductoRepository productoRepository, IMapper mapper)
    {
        _productoRepository = productoRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductoDto>> ObtenerProductosAsync(CancellationToken cancellationToken = default)
    {
        var productos = await _productoRepository.GetAllProductosAsync(cancellationToken);
        return _mapper.Map<List<ProductoDto>>(productos);
    }

    public async Task<ProductoDto?> ObtenerProductoPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var producto = await _productoRepository.GetProductoByIdAsync(id, cancellationToken);
        return producto is null ? null : _mapper.Map<ProductoDto>(producto);
    }

    public async Task<Respuesta<ProductoDto>> CrearProductoAsync(ProductoDto producto, CancellationToken cancellationToken = default)
    {
        var respuesta = new Respuesta<ProductoDto>();
        var error = await ValidarProductoAsync(producto, cancellationToken);
        if (error is not null)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = error;
            respuesta.Codigo = 400;
            return respuesta;
        }

        var entidad = _mapper.Map<Producto>(producto);
        entidad.Id = 0;
        entidad.Nombre = producto.Nombre.Trim();
        if (!await _productoRepository.CreateProductoAsync(entidad, cancellationToken))
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "No se pudo crear el producto.";
            respuesta.Codigo = 500;
            return respuesta;
        }

        respuesta.Mensaje = "Producto registrado correctamente.";
        respuesta.Dato = _mapper.Map<ProductoDto>(entidad);
        return respuesta;
    }

    public async Task<Respuesta<ProductoDto>> ActualizarProductoAsync(ProductoDto producto, CancellationToken cancellationToken = default)
    {
        var respuesta = new Respuesta<ProductoDto>();
        if (producto.Id <= 0 || await _productoRepository.GetProductoByIdAsync(producto.Id, cancellationToken) is null)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "El producto no existe.";
            respuesta.Codigo = 404;
            return respuesta;
        }

        var error = await ValidarProductoAsync(producto, cancellationToken);
        if (error is not null)
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = error;
            respuesta.Codigo = 400;
            return respuesta;
        }

        var entidad = _mapper.Map<Producto>(producto);
        entidad.Nombre = producto.Nombre.Trim();
        var guardado = await _productoRepository.UpdateProductoAsync(entidad, cancellationToken);
        respuesta.Mensaje = guardado ? "Producto actualizado correctamente." : "No hubo cambios para guardar.";
        respuesta.Dato = producto;
        return respuesta;
    }

    public async Task<Respuesta<ProductoDto>> EliminarProductoAsync(int id, CancellationToken cancellationToken = default)
    {
        var respuesta = new Respuesta<ProductoDto>();
        if (id <= 0 || !await _productoRepository.DeleteProductoAsync(id, cancellationToken))
        {
            respuesta.EsCorrecto = false;
            respuesta.Mensaje = "El producto no existe o no pudo eliminarse.";
            respuesta.Codigo = 404;
            return respuesta;
        }

        respuesta.Mensaje = "Producto eliminado correctamente.";
        return respuesta;
    }

    private async Task<string?> ValidarProductoAsync(ProductoDto producto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(producto.Nombre))
            return "El nombre del producto es obligatorio.";
        if (producto.Nombre.Trim().Length > 150)
            return "El nombre no puede superar los 150 caracteres.";
        if (producto.CategoriaId <= 0 || !await _productoRepository.ExisteCategoriaAsync(producto.CategoriaId, cancellationToken))
            return "Debe seleccionar una categoría existente.";
        return null;
    }
}
