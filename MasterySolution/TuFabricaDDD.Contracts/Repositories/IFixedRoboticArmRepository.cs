// Ruta: TuFabricaDDD.Contracts/Repositories/IFixedRoboticArmRepository.cs
using TuFabricaDDD.Domain.Entities;

namespace TuFabricaDDD.Contracts.Repositories;

public interface IFixedRoboticArmRepository : IGenericRepository<FixedRoboticArm>
{
    // Añade métodos específicos para FixedRoboticArm si son necesarios, por ejemplo:
    // Task<IEnumerable<FixedRoboticArm>> GetArmsByMountingPoint(string mountingPointId);
}