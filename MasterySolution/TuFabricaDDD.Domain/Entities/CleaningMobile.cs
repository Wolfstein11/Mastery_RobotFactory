// Ruta: TuFabricaDDD.Domain
using FluentResults;
using System;
using System.Collections.Generic;
using TuFabricaDDD.Domain.Common;
using TuFabricaDDD.Domain.Entities;
using TuFabricaDDD.Domain.Errors; // ¡Nuevo using!
using TuFabricaDDD.Domain.Rules;
using TuFabricaDDD.Domain.Types;
using TuFabricaDDD.Domain.ValueObjects;

namespace TuFabricaDDD.Domain.Entities;

public sealed class CleaningMobile : MobileRobot
{
    public List<string> EquippedCleaningTools { get; private set; }
    public double DetergentLevelPercentage { get; private set; }
    public double WaterLevelPercentage { get; private set; }
    public double DustbinCapacityPercentage { get; private set; }

    private CleaningMobile() : base() { }

    public CleaningMobile(
        Guid id,
        string serialNumber,
        Location initialLocation,
        NetworkLocation initialNetworkLocation,
        List<string> equippedCleaningTools)
        : base(id, serialNumber, RobotCategory.CleaningMobile, initialLocation, initialNetworkLocation, LocomotionType.Wheeled)
    {
        if (equippedCleaningTools == null || equippedCleaningTools.Count == 0)
            throw new ArgumentNullException(nameof(equippedCleaningTools), new InvalidArgumentError(nameof(equippedCleaningTools),"Un robot de limpieza debe tener al menos una herramienta de limpieza equipada.").Message);

        EquippedCleaningTools = new List<string>(equippedCleaningTools);
        DetergentLevelPercentage = 100.0;
        WaterLevelPercentage = 100.0;
        DustbinCapacityPercentage = 0.0;
    }

    public Result StartCleaningTask(string areaDescription)
    {
        if (string.IsNullOrWhiteSpace(areaDescription))
            return Result.Fail(new InvalidArgumentError(nameof(areaDescription), "La descripción del área a limpiar no puede ser nula o vacía."));

        var rules = new IBusinessRule[]
        {
            new BatteryLevelMustBeSufficientRule(BatteryLevelPercentage, 20),
            new DetergentLevelMustBeSufficientRule(DetergentLevelPercentage, 10),
            new WaterLevelMustBeSufficientRule(WaterLevelPercentage, 10),
            new DustbinCapacityMustBeSufficientRule(DustbinCapacityPercentage, 90),
            new RobotNotBrokenRule(Status)
        };

        var result = CheckRules(rules);
        if (result.IsFailed)
        {
            return result;
        }

        ChangeStatus(RobotStatus.Busy);
        BatteryLevelPercentage -= 15;
        DetergentLevelPercentage -= 5;
        WaterLevelPercentage -= 5;
        DustbinCapacityPercentage += 10;

        if (BatteryLevelPercentage < 0) BatteryLevelPercentage = 0;
        if (DetergentLevelPercentage < 0) DetergentLevelPercentage = 0;
        if (WaterLevelPercentage < 0) WaterLevelPercentage = 0;
        if (DustbinCapacityPercentage > 100) DustbinCapacityPercentage = 100;

        ChangeStatus(RobotStatus.Idle);
        return Result.Ok().WithSuccess($"Robot de limpieza {SerialNumber} ha completado la tarea en '{areaDescription}'.");
    }

    public Result RefillSupplies()
    {
        DetergentLevelPercentage = 100.0;
        WaterLevelPercentage = 100.0;
        return Result.Ok().WithSuccess($"Robot de limpieza {SerialNumber} ha recargado sus suministros de agua y detergente.");
    }

    public Result EmptyDustbin()
    {
        if (DustbinCapacityPercentage == 0)
        {
            return Result.Fail(new RobotOperationError(SerialNumber, "vaciar papelera", "La papelera ya está vacía."));
        }
        DustbinCapacityPercentage = 0.0;
        return Result.Ok().WithSuccess($"La papelera del robot de limpieza {SerialNumber} ha sido vaciada.");
    }
}