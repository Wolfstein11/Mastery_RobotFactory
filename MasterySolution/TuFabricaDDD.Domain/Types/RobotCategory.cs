using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Ruta: TuFabricaDDD.Domain/Types/RobotCategory.cs
namespace TuFabricaDDD.Domain.Types;

public enum RobotCategory
{
    /// <summary>
    /// Brazos robóticos fijos utilizados para tareas de precisión en un punto específico.
    /// </summary>
    FixedRoboticArm,

    /// <summary>
    /// Robots humanoides, versátiles para tareas que requieren destreza similar a la humana.
    /// </summary>
    Humanoid,

    /// <summary>
    /// Robots móviles encargados de la logística y el transporte de materiales.
    /// </summary>
    LogisticsMobile,

    /// <summary>
    /// Robots móviles dedicados a la seguridad y vigilancia de las instalaciones.
    /// </summary>
    //SecurityMobile,

    /// <summary>
    /// Robots móviles especializados en tareas de limpieza y mantenimiento del área.
    /// </summary>
    CleaningMobile
}