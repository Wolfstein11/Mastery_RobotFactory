// Ruta: TuFabricaDDD.Contracts/Repositories/IRobotRepository.cs
using TuFabricaDDD.Domain.Entities; // Para acceder a la entidad Robot y sus enums
using TuFabricaDDD.Domain.Types; // Para RobotCategory
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TuFabricaDDD.Contracts.Repositories;

/// <summary>
/// Define el contrato para el repositorio específico de robots.
/// Hereda de IGenericRepository para operaciones CRUD básicas y añade métodos específicos para Robot.
/// </summary>
public interface IRobotRepository : IGenericRepository<Robot>
{
    /// <summary>
    /// Obtiene un robot por su número de serie.
    /// </summary>
    /// <param name="serialNumber">El número de serie del robot.</param>
    /// <returns>El robot encontrado, o null si no existe.</returns>
    Task<Robot?> GetBySerialNumberAsync(string serialNumber);

    /// <summary>
    /// Obtiene todos los robots de una categoría específica.
    /// </summary>
    /// <param name="category">La categoría del robot.</param>
    /// <returns>Una colección de robots de la categoría especificada.</returns>
    Task<IEnumerable<Robot>> GetRobotsByCategoryAsync(RobotCategory category);

    // Puedes añadir más métodos específicos de consulta aquí, si el dominio los requiere.
    // Ej: Task<IEnumerable<MobileRobot>> GetMobileRobotsInLocation(Location location);
}