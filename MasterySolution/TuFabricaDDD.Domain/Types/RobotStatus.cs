using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Ruta: TuFabricaDDD.Domain/Types/RobotStatus.cs
namespace TuFabricaDDD.Domain.Types;

public enum RobotStatus
{
    /// <summary>
    /// El robot está en pleno funcionamiento y disponible para tareas.
    /// </summary>
    Operational,

    /// <summary>
    /// El robot está actualmente realizando una tarea asignada.
    /// </summary>
    Busy,

    /// <summary>
    /// El robot está siendo sometido a tareas de mantenimiento.
    /// </summary>
    UnderMaintenance,

    /// <summary>
    /// El robot ha sufrido una avería y no puede operar.
    /// </summary>
    Broken,

    /// <summary>
    /// El robot está inactivo y esperando una nueva asignación.
    /// </summary>
    Idle
}
