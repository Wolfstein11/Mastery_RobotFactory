// Ruta: TuFabricaDDD.Persistence/Repositories/EfCoreRobotRepository.cs
using Microsoft.EntityFrameworkCore;
using TuFabricaDDD.Contracts.Repositories; // Para IRobotRepository
using TuFabricaDDD.Domain.Entities; // Para Robot
using TuFabricaDDD.Domain.Types; // Para RobotCategory
using TuFabricaDDD.Persistence.Contexts; // Para AppDbContext
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TuFabricaDDD.Persistence.Repositories;

public class EfCoreRobotRepository : GenericRepository<Robot>, IRobotRepository
{
    public EfCoreRobotRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Robot?> GetBySerialNumberAsync(string serialNumber)
    {
        // Incluye los Owned Types al buscar por número de serie
        return await _dbSet
            .Include(r => r.CurrentLocation)
            .Include(r => r.NetworkLocation)
            .SingleOrDefaultAsync(r => r.SerialNumber == serialNumber);
    }

    public async Task<IEnumerable<Robot>> GetRobotsByCategoryAsync(RobotCategory category)
    {
        // Incluye los Owned Types al buscar por categoría
        return await _dbSet
            .Where(r => r.Category == category)
            .Include(r => r.CurrentLocation)
            .Include(r => r.NetworkLocation)
            .ToListAsync();
    }

    // Sobreescribe GetByIdAsync para incluir los Owned Types automáticamente
    public override async Task<Robot?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(r => r.CurrentLocation)
            .Include(r => r.NetworkLocation)
            .SingleOrDefaultAsync(r => r.Id == id);
    }

    // Sobreescribe GetAllAsync para incluir los Owned Types automáticamente
    public override async Task<IEnumerable<Robot>> GetAllAsync()
    {
        return await _dbSet
            .Include(r => r.CurrentLocation)
            .Include(r => r.NetworkLocation)
            .ToListAsync();
    }
}