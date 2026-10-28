using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Domain;
using WebAvanzada3Cuatrimestre.Infrastructure.Data;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly ApplicationDbContext _context;

    public CategoriaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> GetAllCategoriasAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Categorias
            .AsNoTracking()
            .ToListAsync(cancellationToken);
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

    public async Task<bool> CreateCategoriaAsync(
        Categoria categoria,
        CancellationToken cancellationToken = default)
    {
        await _context.Categorias.AddAsync(categoria, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
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

        categoriaExistente.Nombre = categoria.Nombre;

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteCategoriaAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (categoria is null)
        {
            return false;
        }

        _context.Categorias.Remove(categoria);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}