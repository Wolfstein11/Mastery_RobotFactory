// Ruta: TuFabricaDDD.Domain/Errors/InvalidArgumentError.cs
using FluentResults;

namespace TuFabricaDDD.Domain.Errors;

/// <summary>
/// Representa un error cuando un argumento proporcionado es nulo, vacío o fuera de un rango válido.
/// </summary>
public class InvalidArgumentError : Error
{
    public InvalidArgumentError(string argumentName, string reason)
        : base($"El argumento '{argumentName}' es inválido. Razón: {reason}.")
    {
        Metadata.Add("ArgumentName", argumentName);
        Metadata.Add("Reason", reason);
    }
}