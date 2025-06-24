// Ruta: TuFabricaDDD.Contracts/Repositories/IRobotStateChangeRecordRepository.cs
using TuFabricaDDD.Domain.Records; // Para RobotStateChangeRecord
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TuFabricaDDD.Contracts.Repositories;

/// <summary>
/// Define el contrato para el repositorio de registros de cambio de estado de robots.
/// </summary>
public interface IRobotStateChangeRecordRepository
{
    /// <summary>
    /// Añade un nuevo registro de cambio de estado de robot.
    /// </summary>
    /// <param name="record">El registro a añadir.</param>
    Task AddAsync(RobotStateChangeRecord record);

    /// <summary>
    /// Obtiene todos los registros de cambio de estado para un robot específico.
    /// </summary>
    /// <param name="robotId">El ID del robot.</param>
    /// <returns>Una colección de registros de cambio de estado para el robot especificado, ordenados por tiempo.</returns>
    Task<IEnumerable<RobotStateChangeRecord>> GetByRobotIdAsync(Guid robotId);

    /// <summary>
    /// Obtiene un registro específico de cambio de estado por el ID del robot y el tiempo de ocurrencia.
    /// </summary>
    /// <param name="robotId">El ID del robot.</param>
    /// <param name="occurringTime">El tiempo en que ocurrió el cambio.</param>
    /// <returns>El registro encontrado, o null si no existe.</returns>
    Task<RobotStateChangeRecord?> GetByRobotIdAndOccurringTimeAsync(Guid robotId, DateTime occurringTime);
}