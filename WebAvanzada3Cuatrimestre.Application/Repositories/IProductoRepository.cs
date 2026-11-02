using WebAvanzada3Cuatrimestre.Domain;

namespace WebAvanzada3Cuatrimestre.Application.Repositories;

public interface IProductoRepository
{
    Task<List<Producto>> GetAllProductosAsync(CancellationToken cancellationToken = default);
    Task<Producto?> GetProductoByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExisteCategoriaAsync(int categoriaId, CancellationToken cancellationToken = default);
    Task<bool> CreateProductoAsync(Producto producto, CancellationToken cancellationToken = default);
    Task<bool> UpdateProductoAsync(Producto producto, CancellationToken cancellationToken = default);
    Task<bool> DeleteProductoAsync(int id, CancellationToken cancellationToken = default);
}
