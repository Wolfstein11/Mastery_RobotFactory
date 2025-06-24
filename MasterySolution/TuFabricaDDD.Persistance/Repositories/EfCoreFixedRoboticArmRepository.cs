// Ruta: TuFabricaDDD.Persistence/Repositories/EfCoreFixedRoboticArmRepository.cs
using Microsoft.EntityFrameworkCore;
using TuFabricaDDD.Contracts.Repositories; // Para IFixedRoboticArmRepository
using TuFabricaDDD.Domain.Entities; // Para FixedRoboticArm
using TuFabricaDDD.Persistence.Contexts; // Para AppDbContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuFabricaDDD.Persistence.Repositories;

public class EfCoreFixedRoboticArmRepository : GenericRepository<FixedRoboticArm>, IFixedRoboticArmRepository
{
    public EfCoreFixedRoboticArmRepository(AppDbContext context) : base(context)
    {
    }

    // Sobreescribe los métodos GetAllAsync y GetByIdAsync para asegurar que se filtren por el tipo correcto
    // y se incluyan los Owned Types si es necesario.
    // Aunque GenericRepository ya usa _dbSet (que es context.Set<FixedRoboticArm> aquí),
    // es buena práctica sobrescribir si necesitas inclusiones o filtrado de tipo específico.
    public override async Task<FixedRoboticArm?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(r => r.CurrentLocation) // Propiedades de Robot base
            .Include(r => r.NetworkLocation) // Propiedades de Robot base
            .OfType<FixedRoboticArm>() // Asegura que solo se devuelvan FixedRoboticArm
            .SingleOrDefaultAsync(r => r.Id == id);
    }

    public override async Task<IEnumerable<FixedRoboticArm>> GetAllAsync()
    {
        return await _dbSet
            .Include(r => r.CurrentLocation) // Propiedades de Robot base
            .Include(r => r.NetworkLocation) // Propiedades de Robot base
            .OfType<FixedRoboticArm>() // Asegura que solo se devuelvan FixedRoboticArm
            .ToListAsync();
    }

    // Añade implementaciones de métodos específicos de IFixedRoboticArmRepository si los hubieras definido.
    // Ejemplo:
    // public async Task<IEnumerable<FixedRoboticArm>> GetArmsByMountingPoint(string mountingPointId)
    // {
    //     return await _dbSet.Where(arm => arm.MountingPointId == mountingPointId).ToListAsync();
    // }
}