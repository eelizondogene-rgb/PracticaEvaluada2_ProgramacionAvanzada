using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Application.Repositories;
using WebAvanzada3Cuatrimestre.Domain;
using WebAvanzada3Cuatrimestre.Infrastructure.Data;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<List<Categoria>> GetAllCategoriasAsync(CancellationToken cancellationToken = default)
    {
        return GetAllAsync(cancellationToken);
    }

    public async Task<Categoria?> GetCategoriaByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return new Categoria();
        }

        return await _context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsCategoriaByNombreAsync(
        string nombre,
        int idExcluir,
        CancellationToken cancellationToken = default)
    {
        var nombreNormalizado = nombre.Trim().ToLower();

        return await _context.Categorias
            .AsNoTracking()
            .AnyAsync(
                c => c.Id != idExcluir && c.Nombre.Trim().ToLower() == nombreNormalizado,
                cancellationToken);
    }

    public async Task<bool> TieneProductosAsociadosAsync(
        int categoriaId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Productos
            .AsNoTracking()
            .AnyAsync(p => p.CategoriaId == categoriaId, cancellationToken);
    }

    public Task<bool> CreateCategoriaAsync(Categoria categoria, CancellationToken cancellationToken = default)
    {
        return CreateAsync(categoria, cancellationToken);
    }

    public async Task<bool> UpdateCategoriaAsync(
        Categoria categoria,
        CancellationToken cancellationToken = default)
    {
        var categoriaExistente = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == categoria.Id, cancellationToken);

        if (categoriaExistente is null)
        {
            return false;
        }

        if (categoriaExistente.Nombre == categoria.Nombre)
        {
            return true;
        }

        categoriaExistente.Nombre = categoria.Nombre;

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public Task<bool> DeleteCategoriaAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteAsync(id, cancellationToken);
    }
}
