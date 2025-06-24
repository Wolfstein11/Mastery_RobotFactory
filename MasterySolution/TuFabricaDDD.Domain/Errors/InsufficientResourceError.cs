// Ruta: TuFabricaDDD.Domain/Errors/InsufficientResourceError.cs
using FluentResults;

namespace TuFabricaDDD.Domain.Errors;

/// <summary>
/// Representa un error cuando un robot no tiene suficientes recursos (ej. batería, detergente) para realizar una tarea.
/// </summary>
public class InsufficientResourceError : Error
{
    public InsufficientResourceError(string resourceName, double currentLevel, double requiredLevel)
        : base($"Recurso '{resourceName}' insuficiente ({currentLevel:F1}%). Se requiere al menos {requiredLevel:F1}%.")
    {
        Metadata.Add("ResourceName", resourceName);
        Metadata.Add("CurrentLevel", currentLevel);
        Metadata.Add("RequiredLevel", requiredLevel);
    }
}