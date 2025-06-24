// Ruta: TuFabricaDDD.Domain/Types/LocomotionType.cs
namespace TuFabricaDDD.Domain.Types;

/// <summary>
/// Define los diferentes tipos de sistemas de locomoción para robots móviles.
/// </summary>
public enum LocomotionType
{
    /// <summary>
    /// Desplazamiento sobre ruedas.
    /// </summary>
    Wheeled,

    /// <summary>
    /// Desplazamiento bípedo/humanoide (piernas).
    /// </summary>
    Bipedal,

    /// <summary>
    /// Desplazamiento sobre orugas.
    /// </summary>
    Tracked,

    /// <summary>
    /// Desplazamiento por aire (drones).
    /// </summary>
    Aerial,

    // Puedes añadir más tipos según sea necesario (ej. "Legged" para multípedos)
}
