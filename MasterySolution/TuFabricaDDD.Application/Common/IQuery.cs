// Ruta: TuFabricaDDD.Application/Common/IQuery.cs
using FluentResults; // Para el patrón Resultado
using MediatR;      // Para integrar con MediatR

namespace TuFabricaDDD.Application.Common
{
    /// <summary>
    /// Representa una consulta que devuelve un valor de tipo T, encapsulado en un FluentResults.Result.
    /// </summary>
    /// <typeparam name="T">El tipo del valor que se espera como resultado de la consulta.</typeparam>
    public interface IQuery<T>
        : IRequest<Result<T>>
    {
        // Esta interfaz está vacía intencionalmente; su propósito es el contrato de tipo y la herencia de IRequest.
    }
}
