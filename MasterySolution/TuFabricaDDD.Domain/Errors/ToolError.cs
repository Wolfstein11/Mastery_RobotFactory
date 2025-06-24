// Ruta: TuFabricaDDD.Domain/Errors/ToolError.cs
using FluentResults;

namespace TuFabricaDDD.Domain.Errors;

/// <summary>
/// Representa un error relacionado con la gestión de herramientas del robot.
/// </summary>
public class ToolError : Error
{
    public ToolError(string toolName, string reason)
        : base($"Error con la herramienta '{toolName}'. Razón: {reason}.")
    {
        Metadata.Add("ToolName", toolName);
        Metadata.Add("Reason", reason);
    }
}