// Ruta: TuFabricaDDD.Domain/Common/IAggregateRoot.cs
namespace TuFabricaDDD.Domain.Common;

/// <summary>
/// Interfaz de marcador para identificar la raíz de un agregado de dominio.
/// Las clases que implementan esta interfaz son los puntos de entrada para todas las operaciones
/// transaccionales y de consistencia dentro de un agregado.
/// </summary>
public interface IAggregateRoot
{
    // Una interfaz de marcador no requiere miembros.
    // Su propósito es puramente semántico para el diseño del dominio.
}