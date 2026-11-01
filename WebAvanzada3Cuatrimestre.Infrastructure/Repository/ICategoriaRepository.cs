using WebAvanzada3Cuatrimestre.Domain;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository;

public interface ICategoriaRepository
{
    Task<List<Categoria>> GetAllCategoriasAsync(CancellationToken cancellationToken = default);
    Task<Categoria?> GetCategoriaByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsCategoriaByNombreAsync(string nombre, int idExcluir, CancellationToken cancellationToken = default);
    Task<bool> TieneProductosAsociadosAsync(int categoriaId, CancellationToken cancellationToken = default);
    Task<bool> CreateCategoriaAsync(Categoria categoria, CancellationToken cancellationToken = default);
    Task<bool> UpdateCategoriaAsync(Categoria categoria, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategoriaAsync(int id, CancellationToken cancellationToken = default);
}