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

public sealed class Humanoid : MobileRobot
{
    public bool HasManipulators { get; private set; }
    public string OperatingSystemVersion { get; private set; }
    public List<string> EquippedTools { get; private set; }

    private Humanoid() : base() { }

    public Humanoid(
        Guid id,
        string serialNumber,
        Location initialLocation,
        NetworkLocation initialNetworkLocation,
        bool hasManipulators,
        string operatingSystemVersion)
        : base(id, serialNumber, RobotCategory.Humanoid, initialLocation, initialNetworkLocation, LocomotionType.Bipedal)
    {
        if (string.IsNullOrWhiteSpace(operatingSystemVersion))
            throw new ArgumentNullException(nameof(operatingSystemVersion), new InvalidArgumentError(nameof(operatingSystemVersion), "La versión del sistema operativo no puede ser nula o vacía.").Message);

        HasManipulators = hasManipulators;
        OperatingSystemVersion = operatingSystemVersion;
        EquippedTools = new List<string>();
    }

    public Result PerformComplexAssemblyTask(string assemblyDetails)
    {
        var rules = new List<IBusinessRule>
        {
            new MustHaveManipulatorsRule(HasManipulators),
            new BatteryLevelMustBeSufficientRule(BatteryLevelPercentage, 20)
        };

        var result = CheckRules(rules.ToArray());
        if (result.IsFailed)
        {
            return result;
        }

        ChangeStatus(RobotStatus.Busy);
        BatteryLevelPercentage -= 15;
        if (BatteryLevelPercentage < 0) BatteryLevelPercentage = 0;

        ChangeStatus(RobotStatus.Idle);
        return Result.Ok().WithSuccess($"Humanoide {SerialNumber} completó la tarea de ensamblaje: {assemblyDetails}.");
    }

    public Result EquipTool(string toolName)
    {
        if (string.IsNullOrWhiteSpace(toolName))
            return Result.Fail(new InvalidArgumentError(nameof(toolName), "El nombre de la herramienta no puede ser nulo o vacío."));

        if (EquippedTools.Contains(toolName))
            return Result.Fail(new ToolError(toolName, $"El humanoide {SerialNumber} ya tiene la herramienta '{toolName}' equipada."));

        EquippedTools.Add(toolName);
        return Result.Ok().WithSuccess($"Herramienta '{toolName}' equipada en el humanoide {SerialNumber}.");
    }

    public Result UnequipTool(string toolName)
    {
        if (string.IsNullOrWhiteSpace(toolName))
            return Result.Fail(new InvalidArgumentError(nameof(toolName), "El nombre de la herramienta no puede ser nulo o vacío."));

        if (!EquippedTools.Contains(toolName))
            return Result.Fail(new ToolError(toolName, $"El humanoide {SerialNumber} no tiene la herramienta '{toolName}' equipada."));

        EquippedTools.Remove(toolName);
        return Result.Ok().WithSuccess($"Herramienta '{toolName}' retirada del humanoide {SerialNumber}.");
    }
}