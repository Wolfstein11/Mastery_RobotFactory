// Ruta: TuFabricaDDD.Contracts/Repositories/IGenericRepository.cs
using TuFabricaDDD.Domain.Common; // Para la restricción de tipo IEntity
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TuFabricaDDD.Contracts.Repositories;

/// <summary>
/// Define el contrato genérico para un repositorio que maneja entidades de dominio.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad, que debe ser una clase y heredar de IEntity.</typeparam>
public interface IGenericRepository<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    /// Obtiene una entidad por su identificador único.
    /// </summary>
    /// <param name="id">El ID de la entidad.</param>
    /// <returns>La entidad encontrada, o null si no existe.</returns>
    Task<TEntity?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtiene todas las entidades.
    /// </summary>
    /// <returns>Una colección de todas las entidades.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Busca entidades basándose en una condición.
    /// </summary>
    /// <param name="predicate">La función de predicado para filtrar.</param>
    /// <returns>Una colección de entidades que cumplen la condición.</returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Añade una nueva entidad al repositorio.
    /// </summary>
    /// <param name="entity">La entidad a añadir.</param>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Actualiza una entidad existente en el repositorio.
    /// </summary>
    /// <param name="entity">La entidad a actualizar.</param>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Elimina una entidad del repositorio por su identificador único.
    /// </summary>
    /// <param name="id">El ID de la entidad a eliminar.</param>
    Task DeleteAsync(Guid id);

    // Opcional: Podrías añadir métodos para añadir/actualizar/eliminar colecciones si es necesario.
    // Task AddRangeAsync(IEnumerable<TEntity> entities);
    // Task RemoveRangeAsync(IEnumerable<TEntity> entities);
}