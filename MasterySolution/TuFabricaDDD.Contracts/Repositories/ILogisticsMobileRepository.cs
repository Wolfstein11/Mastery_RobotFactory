// Ruta: TuFabricaDDD.Contracts/Repositories/ILogisticsMobileRepository.cs
using TuFabricaDDD.Domain.Entities;

namespace TuFabricaDDD.Contracts.Repositories;

public interface ILogisticsMobileRepository : IGenericRepository<LogisticsMobile>
{
    // Añade métodos específicos para LogisticsMobile si son necesarios, por ejemplo:
    // Task<IEnumerable<LogisticsMobile>> GetOverloadedLogisticsMobiles(double threshold);
}