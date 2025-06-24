// Ruta: TuFabricaDDD.Application/Common/IQueryHandler.cs
using FluentResults; // Para el patrón Resultado
using MediatR;      // Para integrar con MediatR
using System.Threading;
using System.Threading.Tasks;

namespace TuFabricaDDD.Application.Common
{
    /// <summary>
    /// Representa un manejador para una consulta de tipo TQuery, devolviendo un resultado de tipo TResult.
    /// </summary>
    /// <typeparam name="TQuery">El tipo específico de consulta que este manejador procesa.</typeparam>
    /// <typeparam name="TResult">El tipo del valor que se espera como resultado de la consulta.</typeparam>
    public interface IQueryHandler<TQuery, TResult>
        : IRequestHandler<TQuery, Result<TResult>>
        where TQuery : IQuery<TResult>
    {
        // Esta interfaz está vacía intencionalmente; su propósito es el contrato de tipo y la herencia de IRequestHandler.
    }
}