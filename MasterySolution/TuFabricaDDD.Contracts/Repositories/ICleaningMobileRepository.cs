// Ruta: TuFabricaDDD.Contracts/Repositories/ICleaningMobileRepository.cs
using TuFabricaDDD.Domain.Entities;

namespace TuFabricaDDD.Contracts.Repositories;

public interface ICleaningMobileRepository : IGenericRepository<CleaningMobile>
{
    // Añade métodos específicos para CleaningMobile si son necesarios, por ejemplo:
    // Task<IEnumerable<CleaningMobile>> GetLowDetergentCleaningMobiles();
}