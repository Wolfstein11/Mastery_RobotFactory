// Ruta: TuFabricaDDD.Persistence/Repositories/GenericRepository.cs
using Microsoft.EntityFrameworkCore;
using TuFabricaDDD.Contracts.Repositories; // Para IGenericRepository
using TuFabricaDDD.Domain.Common; // Para IEntity
using TuFabricaDDD.Persistence.Contexts; // Para AppDbContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TuFabricaDDD.Persistence.Repositories;

/// <summary>
/// Implementación genérica de IGenericRepository para operaciones CRUD básicas con Entity Framework Core.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad a manejar, debe ser una clase y heredar de IEntity.</typeparam>
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        await _dbSet.AddAsync(entity);
    }

    public virtual Task UpdateAsync(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _dbSet.Update(entity);
        return Task.CompletedTask; // Update es síncrono en el contexto de EF Core
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);
        if (entityToDelete != null)
        {
            _dbSet.Remove(entityToDelete);
        }
    }
}