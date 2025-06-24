// Ruta: TuFabricaDDD.Application/Commands/Units/CreateUnit/CreateUnitCommand.cs
using TuFabricaDDD.Application.Common;
using TuFabricaDDD.Domain.Types; // Para RobotCategory

namespace TuFabricaDDD.Application.Commands
{
    // Hereda de ICommand<Guid> porque al crear una unidad, queremos devolver su ID.
    // Usamos un record para una definición concisa e inmutable.
    public sealed record CreateUnitCommand(
        string SerialNumber,
        RobotCategory Category,
        string AreaName,
        double CurrentLocationX,
        double CurrentLocationY,
        string NetworkLocationIpAddress,
        string NetworkLocationConnectedAccessPointSsid,
        int NetworkLocationConnectedAccessPointChannel,
        double? ArmLengthInMeters,
        double? PayloadCapacityInKg,
        string? MountingPointId,
        string? LocomotionType
    ) : ICommand<Guid>;
}