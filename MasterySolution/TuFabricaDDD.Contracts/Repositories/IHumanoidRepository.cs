// Ruta: TuFabricaDDD.Contracts/Repositories/IHumanoidRepository.cs
using TuFabricaDDD.Domain.Entities;

namespace TuFabricaDDD.Contracts.Repositories;

public interface IHumanoidRepository : IGenericRepository<Humanoid>
{
    // Añade métodos específicos para Humanoid si son necesarios, por ejemplo:
    // Task<IEnumerable<Humanoid>> GetHumanoidsByOperatingSystem(string osVersion);
}