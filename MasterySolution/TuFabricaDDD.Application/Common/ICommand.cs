// Ruta: TuFabricaDDD.Application/Common/ICommand.cs
using MediatR;
using FluentResults;

namespace TuFabricaDDD.Application.Common
{
    public interface ICommand
        : IRequest<Result> // Comando que no devuelve un valor específico, solo el estado de la operación.
    {

    }

    public interface ICommand<T>
        : IRequest<Result<T>> // Comando que devuelve un valor específico (T) y el estado de la operación.
    {

    }
}