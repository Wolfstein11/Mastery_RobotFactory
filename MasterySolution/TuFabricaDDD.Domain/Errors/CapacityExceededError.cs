// Ruta: TuFabricaDDD.Domain/Errors/CapacityExceededError.cs
using FluentResults;

namespace TuFabricaDDD.Domain.Errors;

/// <summary>
/// Representa un error cuando se intenta exceder la capacidad máxima de un robot o sistema.
/// </summary>
public class CapacityExceededError : Error
{
    public CapacityExceededError(string itemDescription, double attemptedValue, double maxValue)
        : base($"La {itemDescription} de {attemptedValue:F1} excede la capacidad máxima de {maxValue:F1}.")
    {
        Metadata.Add("ItemDescription", itemDescription);
        Metadata.Add("AttemptedValue", attemptedValue);
        Metadata.Add("MaxValue", maxValue);
    }
}