// Ruta: TuFabricaDDD.Application/Queries/GetAllUnitsQueryHandler.cs
using FluentResults;                 // Para Result
using System.Collections.Generic;    // Para IEnumerable
using System.Threading;              // Para CancellationToken
using System.Threading.Tasks;        // Para Task
using TuFabricaDDD.Application.Common; // Para IQueryHandler
using TuFabricaDDD.Contracts.Repositories; // <-- Corregido: Para IRepositoryManager
using TuFabricaDDD.Domain.Entities;   // Para Unit (Robot)

namespace TuFabricaDDD.Application.Queries
{
    // Hereda de IQueryHandler<GetAllUnitsQuery, IEnumerable<Unit>>
    // para indicar que maneja GetAllUnitsQuery y devuelve un IEnumerable de Units.
    public class GetAllUnitsQueryHandler
        : IQueryHandler<GetAllUnitsQuery, IEnumerable<Robot>>
    {
        // Usamos IRepositoryManager, no IAppRepositoryManager, según tu aclaración anterior.
        private readonly IRepositoryManager _repositoryManager;

        public GetAllUnitsQueryHandler(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<Result<IEnumerable<Robot>>> Handle(
            GetAllUnitsQuery request,
            CancellationToken cancellationToken)
        {
            // IRepositoryManager.RobotRepository tiene un método GetUnitsAsync() que devuelve un Task<IEnumerable<Unit>>.
            
            var units = await _repositoryManager.Robots.GetAllAsync();

            // Si necesitas mapear la entidad de dominio 'Robot' a un DTO de consulta específico,
            // lo harías aquí usando AutoMapper. Por ahora, devolvemos la entidad directamente.
            return Result.Ok(units);
        }
    }
}