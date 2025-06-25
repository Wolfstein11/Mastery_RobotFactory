// Ruta: TuFabricaDDD.Domain/Aggregates/LogisticsMobileAggregate/LogisticsMobile.cs
using FluentResults;
using System;
using TuFabricaDDD.Domain.Common;
using TuFabricaDDD.Domain.Entities;
using TuFabricaDDD.Domain.Errors; // ¡Nuevo using!
using TuFabricaDDD.Domain.Rules;
using TuFabricaDDD.Domain.Types;
using TuFabricaDDD.Domain.ValueObjects;

namespace TuFabricaDDD.Domain.Entities;

public sealed class LogisticsMobile : MobileRobot
{
    public double MaxPayloadCapacityInKg { get; private set; }
    private LogisticsMobile() : base() { }

    public LogisticsMobile(
        Guid id,
        string serialNumber,
        Location initialLocation,
        NetworkLocation initialNetworkLocation,
        double maxPayloadCapacityInKg)
        : base(id, serialNumber, RobotCategory.LogisticsMobile, initialLocation, initialNetworkLocation, LocomotionType.Wheeled)
    {
        if (maxPayloadCapacityInKg <= 0)
            throw new ArgumentNullException(nameof(maxPayloadCapacityInKg), new InvalidArgumentError(nameof(maxPayloadCapacityInKg), "La capacidad máxima de carga debe ser mayor que cero.").Message);

        MaxPayloadCapacityInKg = maxPayloadCapacityInKg;
    }

    public Result LoadCargo(double weightInKg)
    {
        var rules = new IBusinessRule[]
        {
            new NonNegativeWeightRule(weightInKg),
            new ExceedsPayloadCapacityRule(MaxPayloadCapacityInKg, weightInKg)
        };

        var result = CheckRules(rules);
        if (result.IsFailed)
        {
            return result;
        }

        ChangeStatus(RobotStatus.Busy);
        ChangeStatus(RobotStatus.Idle);

        return Result.Ok().WithSuccess($"Robot logístico {SerialNumber} cargado con {weightInKg} kg.");
    }
}