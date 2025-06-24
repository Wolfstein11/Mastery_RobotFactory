// Ruta: TuFabricaDDD.Contracts/Repositories/IUnitOfWork.cs
using System.Threading.Tasks;

namespace TuFabricaDDD.Contracts.Repositories;

/// <summary>
/// Define el contrato para la Unidad de Trabajo.
/// Es responsable de encapsular las operaciones de base de datos y de asegurar que un conjunto
/// de cambios se envíe a la base de datos como una sola transacción atómica.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Guarda todos los cambios pendientes en la base de datos de forma asíncrona.
    /// </summary>
    /// <returns>El número de objetos escritos en la base de datos.</returns>
    Task<int> SaveChangesAsync();

    // Opcional: Podrías exponer los repositorios específicos a través de propiedades aquí.
    // Aunque a veces se prefiere inyectar IRobotRepository e IUnitOfWork por separado
    // en los servicios de aplicación para mayor flexibilidad.
    // Ejemplo:
    // IRobotRepository Robots { get; }
    // IRobotStateChangeRecordRepository RobotStateChangeRecords { get; }
}
