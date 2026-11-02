using Microsoft.EntityFrameworkCore;
using WebAvanzada3Cuatrimestre.Application.Repositories;
using WebAvanzada3Cuatrimestre.Domain;
using WebAvanzada3Cuatrimestre.Infrastructure.Data;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository;

public class ProductoRepository : Repository<Producto>, IProductoRepository
{
    public ProductoRepository(ApplicationDbContext context) : base(context)
    {
    }

    protected override IQueryable<Producto> Consulta()
    {
        return _context.Productos.AsNoTracking().Include(p => p.Categoria);
    }

    public Task<List<Producto>> GetAllProductosAsync(CancellationToken cancellationToken = default)
    {
        return GetAllAsync(cancellationToken);
    }

    public Task<Producto?> GetProductoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return GetByIdAsync(id, cancellationToken);
    }

    public Task<bool> ExisteCategoriaAsync(int categoriaId, CancellationToken cancellationToken = default)
    {
        return _context.Categorias.AnyAsync(c => c.Id == categoriaId, cancellationToken);
    }

    public Task<bool> CreateProductoAsync(Producto producto, CancellationToken cancellationToken = default)
    {
        return CreateAsync(producto, cancellationToken);
    }

    public Task<bool> UpdateProductoAsync(Producto producto, CancellationToken cancellationToken = default)
    {
        return UpdateAsync(producto, cancellationToken);
    }

    public Task<bool> DeleteProductoAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteAsync(id, cancellationToken);
    }
}
