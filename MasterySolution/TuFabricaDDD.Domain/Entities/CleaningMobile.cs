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
    }

    public Result StartCleaningTask(string areaDescription)
    {
        if (string.IsNullOrWhiteSpace(areaDescription))
            return Result.Fail(new InvalidArgumentError(nameof(areaDescription), "La descripción del área a limpiar no puede ser nula o vacía."));

        var rules = new IBusinessRule[]
        {
            new BatteryLevelMustBeSufficientRule(BatteryLevelPercentage, 20),
            new RobotNotBrokenRule(Status)
        };

        var result = CheckRules(rules);
        if (result.IsFailed)
        {
            return result;
        }

        ChangeStatus(RobotStatus.Busy);
        BatteryLevelPercentage -= 15;

        if (BatteryLevelPercentage < 0) BatteryLevelPercentage = 0;

        ChangeStatus(RobotStatus.Idle);
        return Result.Ok().WithSuccess($"Robot de limpieza {SerialNumber} ha completado la tarea en '{areaDescription}'.");
    }
}