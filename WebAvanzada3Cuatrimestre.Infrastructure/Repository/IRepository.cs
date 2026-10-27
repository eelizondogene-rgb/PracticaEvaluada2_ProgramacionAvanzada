using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebAvanzada3Cuatrimestre.Infrastructure.Repository;

// Contrato generico del CRUD. Las interfaces especificas (ICarroRepository, IDuennoRepository) heredan de esta
// y solo agregan las consultas propias de cada entidad
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Obtiene todos los registros de la entidad, sin seguimiento de cambios.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la consulta.</param>
    /// <returns>Lista con todos los registros.</returns>
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un registro por su llave primaria, sin seguimiento de cambios.
    /// </summary>
    /// <param name="id">Llave primaria del registro.</param>
    /// <param name="cancellationToken">Token para cancelar la consulta.</param>
    /// <returns>El registro encontrado, o null si no existe o el id no es valido.</returns>
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inserta un nuevo registro.
    /// </summary>
    /// <param name="entidad">Registro a insertar.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>true si se guardo al menos un cambio en la base de datos.</returns>
    Task<bool> CreateAsync(T entidad, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza un registro existente. Las columnas con valor por defecto de BD (p. ej. FechaCreacion) no se sobrescriben.
    /// </summary>
    /// <param name="entidad">Registro con los valores nuevos; se identifica por su llave primaria.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>true si se guardo al menos un cambio; false si el registro no existe o no hubo cambios.</returns>
    Task<bool> UpdateAsync(T entidad, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina un registro por su llave primaria.
    /// </summary>
    /// <param name="id">Llave primaria del registro.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion.</param>
    /// <returns>true si se elimino; false si el registro no existe.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
