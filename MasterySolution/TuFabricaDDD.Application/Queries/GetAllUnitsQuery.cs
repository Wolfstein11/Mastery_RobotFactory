// Ruta: TuFabricaDDD.Application/Queries/GetAllUnitsQuery.cs
using System.Collections.Generic;    // Para IEnumerable
using TuFabricaDDD.Application.Common; // Para IQuery
using TuFabricaDDD.Domain.Entities;   // Para Unit (Robot)

namespace TuFabricaDDD.Application.Queries
{
    // GetAllUnitsQuery no necesita parámetros de entrada por ahora, por eso los paréntesis vacíos.
    // Hereda de IQuery<IEnumerable<Domain.Entities.Unit>> para indicar que devuelve una colección de Units.
    public sealed record GetAllUnitsQuery()
        : IQuery<IEnumerable<Robot>>; // Usamos 'Unit' directamente si 'using TuFabricaDDD.Domain.Entities;' está presente.
}