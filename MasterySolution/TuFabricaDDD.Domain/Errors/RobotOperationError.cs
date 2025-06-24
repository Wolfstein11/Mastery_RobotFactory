// Ruta: TuFabricaDDD.Domain/Errors/RobotOperationError.cs
using FluentResults;

namespace TuFabricaDDD.Domain.Errors;

/// <summary>
/// Representa un error cuando un robot no puede realizar una operación solicitada debido a su estado o tipo.
/// </summary>
public class RobotOperationError : Error
{
    public RobotOperationError(string robotSerialNumber, string operationAttempted, string reason)
        : base($"El robot '{robotSerialNumber}' no pudo realizar la operación '{operationAttempted}'. Razón: {reason}.")
    {
        Metadata.Add("RobotSerialNumber", robotSerialNumber);
        Metadata.Add("OperationAttempted", operationAttempted);
        Metadata.Add("Reason", reason);
    }
}