// Ruta: TuFabricaDDD.Domain/Common/CheckableObject.cs
using FluentResults;
using TuFabricaDDD.Domain.Rules; // Necesitamos esta referencia para IBusinessRule
using System.Collections.Generic;
using System.Linq; // Para Result.Merge

namespace TuFabricaDDD.Domain.Common;

/// <summary>
/// Clase base abstracta para objetos del dominio que pueden ser validados
/// contra un conjunto de reglas de negocio. Utiliza el Patrón Resultado (FluentResults)
/// para devolver los errores de validación.
/// </summary>
public abstract class CheckableObject
{
    /// <summary>
    /// Constructor protegido para permitir la instanciación por clases derivadas.
    /// </summary>
    protected CheckableObject() { }

    /// <summary>
    /// Evalúa un conjunto de reglas de negocio y fusiona sus resultados.
    /// Este método es estático y protegido, lo que significa que las clases derivadas
    /// lo invocarán directamente pasando las reglas a verificar.
    /// </summary>
    /// <param name="rules">Un array de reglas de negocio a evaluar.</param>
    /// <returns>
    /// Un <see cref="Result"/> que es la fusión de los resultados de todas las reglas.
    /// Si alguna regla falla, el resultado final será un fallo con todos los errores acumulados.
    /// </returns>
    protected static Result CheckRules(params IBusinessRule[] rules)
    {
        List<Result> results = new List<Result>();
        foreach (var rule in rules)
        {
            // Cada regla es responsable de su propia validación y de devolver su Result.
            results.Add(rule.CheckRule());
        }
        // Fusiona todos los resultados individuales en un único resultado.
        return Result.Merge(results.ToArray());
    }
}
