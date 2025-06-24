// Ruta: TuFabricaDDD.Domain/Records/RobotStateChangeRecord.cs
using TuFabricaDDD.Domain.Types; // Necesario para RobotStatus
using System;

namespace TuFabricaDDD.Domain.Records;

/// <summary>
/// Registro de un cambio en el estado de un robot.
/// </summary>
/// <param name="RobotId">El identificador único del robot.</param>
/// <param name="OccurringTime">La fecha y hora en que ocurrió el cambio de estado.</param>
/// <param name="OldStatus">El estado anterior del robot.</param>
/// <param name="NewStatus">El nuevo estado del robot.</param>
/// <param name="Reason">Una breve descripción de la razón del cambio de estado (opcional).</param>
public record RobotStateChangeRecord(
    Guid RobotId,
    DateTime OccurringTime,
    RobotStatus OldStatus,
    RobotStatus NewStatus,
    string? Reason = null); // El '?' indica que Reason es opcional y puede ser nulo