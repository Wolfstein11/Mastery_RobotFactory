// Ruta: TuFabricaDDD.Domain/Errors/BusinessRuleError.cs
using FluentResults;

namespace TuFabricaDDD.Domain.Errors;

/// <summary>
/// Representa un error cuando una regla de negocio específica es violada.
/// </summary>
public class BusinessRuleError : Error
{
    public BusinessRuleError(string message)
        : base(message)
    {
        Metadata.Add("Category", "Business Rule Violation");
    }
}