// Ruta: TuFabricaDDD.Domain
using TuFabricaDDD.Domain.Entities;
using TuFabricaDDD.Domain.ValueObjects;
using TuFabricaDDD.Domain.Types;
using TuFabricaDDD.Domain.Rules;
using TuFabricaDDD.Domain.Errors; // ¡Nuevo using!
using FluentResults;
using System;
using System.Collections.Generic;

namespace TuFabricaDDD.Domain.Entities;

public sealed class FixedRoboticArm : Robot
{
    public double ArmLengthInMeters { get; private set; }
    public double PayloadCapacityInKg { get; private set; }
    public string MountingPointId { get; private set; }

    private FixedRoboticArm() : base() { }

    public FixedRoboticArm(
        Guid id,
        string serialNumber,
        Location initialLocation,
        NetworkLocation initialNetworkLocation,
        double armLengthInMeters,
        double payloadCapacityInKg,
        string mountingPointId)
        : base(id, serialNumber, RobotCategory.FixedRoboticArm, initialLocation, initialNetworkLocation)
    {
        if (armLengthInMeters <= 0)
            throw new ArgumentNullException(nameof(armLengthInMeters), new InvalidArgumentError(nameof(armLengthInMeters), "La longitud del brazo debe ser mayor que cero.").Message);
        if (payloadCapacityInKg <= 0)
            throw new ArgumentNullException(nameof(payloadCapacityInKg), new InvalidArgumentError(nameof(payloadCapacityInKg),"La capacidad de carga debe ser mayor que cero.").Message);
        if (string.IsNullOrWhiteSpace(mountingPointId))
            throw new ArgumentNullException(nameof(mountingPointId), new InvalidArgumentError(nameof(mountingPointId), "El ID del punto de montaje no puede ser nulo o vacío.").Message);

        ArmLengthInMeters = armLengthInMeters;
        PayloadCapacityInKg = payloadCapacityInKg;
        MountingPointId = mountingPointId;
    }

    public Result PerformManipulationTask(string taskDescription, double weightInKg)
    {
        var weightRule = new MaxPayloadCapacityRule(PayloadCapacityInKg, weightInKg);
        var result = CheckRules(weightRule);

        if (result.IsFailed)
        {
            return result;
        }

        ChangeStatus(RobotStatus.Busy);
        ChangeStatus(RobotStatus.Idle);

        return Result.Ok().WithSuccess($"Brazo robótico {SerialNumber} completó la tarea: {taskDescription}.");
    }

    public override Result UpdateLocation(Location newLocation) // Usa override para dejar claro que se sobreescribe
    {
        // Un brazo fijo no puede cambiar su ubicación física.
        return Result.Fail(new RobotOperationError(SerialNumber, "actualizar ubicación", $"Un brazo robótico fijo no puede cambiar su ubicación de {CurrentLocation} a {newLocation}."));
    }
}