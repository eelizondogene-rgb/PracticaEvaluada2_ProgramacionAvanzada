using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAvanzada3Cuatrimestre.Infrastructure.Data;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository;

// Implementacion generica del CRUD con EF Core. Asume una llave primaria simple de tipo int.
// Los repositorios especificos heredan de esta clase y solo sobrescriben lo que cambia (p. ej. Consulta() para hacer Include)
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    // Nombre de la llave primaria leido del modelo de EF, asi las entidades del dominio no necesitan implementar nada
    private readonly string _nombreLlave;

    public Repository(ApplicationDbContext context) // Inyeccion de dependencias
    {
        _context = context;
        _dbSet = context.Set<T>();
        _nombreLlave = context.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!.Properties[0].Name;
    }

    /// <summary>
    /// Consulta base de lectura: sin seguimiento de cambios. Los repositorios hijos la sobrescriben para agregar Include.
    /// Es protected para no exponer IQueryable fuera de la capa de infraestructura.
    /// </summary>
    /// <returns>IQueryable de la entidad, aun sin ejecutar.</returns>
    protected virtual IQueryable<T> Consulta()
    {
        return _dbSet.AsNoTracking();
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Consulta().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return null;
        }

        return await Consulta()
            .FirstOrDefaultAsync(e => EF.Property<int>(e, _nombreLlave) == id, cancellationToken);
    }

    public async Task<bool> CreateAsync(T entidad, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entidad, cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> UpdateAsync(T entidad, CancellationToken cancellationToken = default)
    {
        var id = _context.Entry(entidad).Property(_nombreLlave).CurrentValue;

        // Se usa el arreglo [id] para que el token no se interprete como una segunda llave
        var existente = await _dbSet.FindAsync([id], cancellationToken);

        if (existente is null)
        {
            return false;
        }

        var entrada = _context.Entry(existente);
        entrada.CurrentValues.SetValues(entidad);

        // Las columnas que llena la BD (DEFAULT, p. ej. FechaCreacion) no vienen en el DTO; se conserva su valor original
        foreach (var propiedad in entrada.Properties.Where(p => p.Metadata.GetDefaultValueSql() != null))
        {
            propiedad.CurrentValue = propiedad.OriginalValue;
            propiedad.IsModified = false;
        }

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existente = await _dbSet.FindAsync([id], cancellationToken);

        if (existente is null)
        {
            return false;
        }

        _dbSet.Remove(existente);
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
