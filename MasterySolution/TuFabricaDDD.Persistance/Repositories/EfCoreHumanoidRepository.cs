// Ruta: TuFabricaDDD.Persistence/Repositories/EfCoreHumanoidRepository.cs
using Microsoft.EntityFrameworkCore;
using TuFabricaDDD.Contracts.Repositories; // Para IHumanoidRepository
using TuFabricaDDD.Domain.Entities; // Para Humanoid
using TuFabricaDDD.Persistence.Contexts; // Para AppDbContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuFabricaDDD.Persistence.Repositories;

public class EfCoreHumanoidRepository : GenericRepository<Humanoid>, IHumanoidRepository
{
    public EfCoreHumanoidRepository(AppDbContext context) : base(context)
    {
    }

    // Sobreescribe los métodos GetAllAsync y GetByIdAsync para asegurar que se filtren por el tipo correcto
    // y se incluyan los Owned Types.
    public override async Task<Humanoid?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(r => r.CurrentLocation)
            .Include(r => r.NetworkLocation)
            .OfType<Humanoid>()
            .SingleOrDefaultAsync(r => r.Id == id);
    }

    public override async Task<IEnumerable<Humanoid>> GetAllAsync()
    {
        return await _dbSet
            .Include(r => r.CurrentLocation)
            .Include(r => r.NetworkLocation)
            .OfType<Humanoid>()
            .ToListAsync();
    }
    // Implementa métodos específicos de IHumanoidRepository si los hubieras definido.
}
