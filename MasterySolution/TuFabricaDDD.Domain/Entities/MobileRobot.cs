// Ruta: TuFabricaDDD.Domain/Entities/MobileRobot.cs
using TuFabricaDDD.Domain.Common;
using TuFabricaDDD.Domain.ValueObjects;
using TuFabricaDDD.Domain.Types;
using TuFabricaDDD.Domain.Rules;
using TuFabricaDDD.Domain.Errors; // ¡Nuevo using!
using FluentResults;
using System;
using System.Collections.Generic;

namespace TuFabricaDDD.Domain.Entities;

public abstract class MobileRobot : Robot
{
    public double BatteryLevelPercentage { get; protected set; }
    public LocomotionType Locomotion { get; protected set; }

    protected MobileRobot() : base() { }

    protected MobileRobot(
        Guid id,
        string serialNumber,
        RobotCategory category,
        Location initialLocation,
        NetworkLocation initialNetworkLocation,
        LocomotionType locomotion)
        : base(id, serialNumber, category, initialLocation, initialNetworkLocation)
    {
        BatteryLevelPercentage = 100.0;
        Locomotion = locomotion;
    }

    public Result ChargeBattery(double chargeAmountPercentage)
    {
        if (chargeAmountPercentage <= 0)
            return Result.Fail(new InvalidArgumentError(nameof(chargeAmountPercentage), "La cantidad de carga debe ser positiva."));

        var chargeRule = new BatteryNotExceedMaxRule(BatteryLevelPercentage, chargeAmountPercentage);
        var result = CheckRules(chargeRule);
        if (result.IsFailed)
        {
            return result;
        }

        ChangeStatus(RobotStatus.UnderMaintenance);
        BatteryLevelPercentage += chargeAmountPercentage;
        if (BatteryLevelPercentage > 100) BatteryLevelPercentage = 100;

        ChangeStatus(RobotStatus.Idle);
        return Result.Ok().WithSuccess($"Robot móvil {SerialNumber} cargó la batería al {BatteryLevelPercentage:F1}%.");
    }

    public Result NavigateTo(Location targetLocation) // Ahora override
    {
        var rules = new IBusinessRule[]
        {
            new BatteryLevelMustBeSufficientRule(BatteryLevelPercentage, 5),
            new RobotNotBrokenRule(Status)
        };

        var result = CheckRules(rules);
        if (result.IsFailed)
        {
            return result;
        }

        ChangeStatus(RobotStatus.Busy);
        BatteryLevelPercentage -= 2;
        if (BatteryLevelPercentage < 0) BatteryLevelPercentage = 0;

        var locationUpdateResult = base.UpdateLocation(targetLocation);
        if (locationUpdateResult.IsFailed) return locationUpdateResult;

        ChangeStatus(RobotStatus.Idle);
        return Result.Ok().WithSuccess($"Robot móvil {SerialNumber} se movió a {targetLocation}. Batería restante: {BatteryLevelPercentage:F1}%.");
    }
}