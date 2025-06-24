// Ruta: TuFabricaDDD.Domain/Rules/IBusinessRule.cs
using FluentResults; // Asegúrate de tener instalado el paquete NuGet FluentResults

namespace TuFabricaDDD.Domain.Common;

/// <summary>
/// Interfaz para definir una regla de negocio.
/// Ahora incluye un método para verificar la regla y devolver un FluentResult.
/// </summary>
public interface IBusinessRule
{
    /// <summary>
    /// Verifica si la regla de negocio está rota.
    /// (Puede implementarse delegando a CheckRule().IsFailed)
    /// </summary>
    /// <returns>True si la regla está rota, de lo contrario, False.</returns>
    bool IsBroken();

    /// <summary>
    /// Obtiene un mensaje descriptivo si la regla de negocio está rota.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Evalúa la regla de negocio y devuelve un FluentResult.
    /// Si la regla está rota, el resultado contendrá los errores correspondientes.
    /// </summary>
    /// <returns>Un <see cref="Result"/> indicando el éxito o fallo de la regla.</returns>
    Result CheckRule();
}
