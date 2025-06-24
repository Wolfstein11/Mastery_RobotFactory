// Ruta: TuFabricaDDD.Persistence/Repositories/EfCoreLogisticsMobileRepository.cs
using Microsoft.EntityFrameworkCore;
using TuFabricaDDD.Contracts.Repositories; // Para ILogisticsMobileRepository
using TuFabricaDDD.Domain.Entities; // Para LogisticsMobile
using TuFabricaDDD.Persistence.Contexts; // Para AppDbContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuFabricaDDD.Persistence.Repositories;

public class EfCoreLogisticsMobileRepository : GenericRepository<LogisticsMobile>, ILogisticsMobileRepository
{
    public EfCoreLogisticsMobileRepository(AppDbContext context) : base(context)
    {
    }

    // Sobreescribe los métodos GetAllAsync y GetByIdAsync para asegurar que se filtren por el tipo correcto
    // y se incluyan los Owned Types.
    public override async Task<LogisticsMobile?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(r => r.CurrentLocation)
            .Include(r => r.NetworkLocation)
            .OfType<LogisticsMobile>()
            .SingleOrDefaultAsync(r => r.Id == id);
    }

    public override async Task<IEnumerable<LogisticsMobile>> GetAllAsync()
    {
        return await _dbSet
            .Include(r => r.CurrentLocation)
            .Include(r => r.NetworkLocation)
            .OfType<LogisticsMobile>()
            .ToListAsync();
    }
    // Implementa métodos específicos de ILogisticsMobileRepository si los hubieras definido.
}
