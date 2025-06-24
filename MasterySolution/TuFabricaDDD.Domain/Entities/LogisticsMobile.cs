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
    public double CurrentPayloadWeightInKg { get; private set; }
    public string NavigationSystemType { get; private set; }
    public string CurrentCargoDescription { get; private set; }

    private LogisticsMobile() : base() { }

    public LogisticsMobile(
        Guid id,
        string serialNumber,
        Location initialLocation,
        NetworkLocation initialNetworkLocation,
        double maxPayloadCapacityInKg,
        string navigationSystemType)
        : base(id, serialNumber, RobotCategory.LogisticsMobile, initialLocation, initialNetworkLocation, LocomotionType.Wheeled)
    {
        if (maxPayloadCapacityInKg <= 0)
            throw new ArgumentNullException(nameof(maxPayloadCapacityInKg), new InvalidArgumentError(nameof(maxPayloadCapacityInKg),"La capacidad máxima de carga debe ser mayor que cero.").Message);
        if (string.IsNullOrWhiteSpace(navigationSystemType))
            throw new ArgumentNullException(nameof(navigationSystemType), new InvalidArgumentError(nameof(navigationSystemType),"El tipo de sistema de navegación no puede ser nulo o vacío.").Message);

        MaxPayloadCapacityInKg = maxPayloadCapacityInKg;
        CurrentPayloadWeightInKg = 0;
        NavigationSystemType = navigationSystemType;
        CurrentCargoDescription = string.Empty;
    }

    public Result LoadCargo(double weightInKg, string cargoDescription)
    {
        var rules = new IBusinessRule[]
        {
            new NonNegativeWeightRule(weightInKg),
            new ExceedsPayloadCapacityRule(MaxPayloadCapacityInKg, CurrentPayloadWeightInKg + weightInKg)
        };

        var result = CheckRules(rules);
        if (result.IsFailed)
        {
            return result;
        }

        ChangeStatus(RobotStatus.Busy);
        CurrentPayloadWeightInKg += weightInKg;
        CurrentCargoDescription = cargoDescription;
        ChangeStatus(RobotStatus.Idle);

        return Result.Ok().WithSuccess($"Robot logístico {SerialNumber} cargado con {weightInKg} kg de '{cargoDescription}'.");
    }

    public Result UnloadCargo(double unloadedWeightInKg = -1)
    {
        if (CurrentPayloadWeightInKg <= 0)
        {
            return Result.Fail(new RobotOperationError(SerialNumber, "descargar carga", "El robot no tiene carga para descargar."));
        }
        if (unloadedWeightInKg < 0 && unloadedWeightInKg != -1)
        {
            return Result.Fail(new InvalidArgumentError(nameof(unloadedWeightInKg), "El peso a descargar no puede ser negativo (excepto para descarga completa)."));
        }

        double actualUnloadWeight = (unloadedWeightInKg == -1) ? CurrentPayloadWeightInKg : unloadedWeightInKg;

        if (actualUnloadWeight > CurrentPayloadWeightInKg)
        {
            return Result.Fail(new CapacityExceededError("cantidad a descargar", actualUnloadWeight, CurrentPayloadWeightInKg));
        }

        ChangeStatus(RobotStatus.Busy);
        CurrentPayloadWeightInKg -= actualUnloadWeight;
        if (CurrentPayloadWeightInKg < 0) CurrentPayloadWeightInKg = 0;

        if (CurrentPayloadWeightInKg == 0)
        {
            CurrentCargoDescription = string.Empty;
        }

        ChangeStatus(RobotStatus.Idle);
        return Result.Ok().WithSuccess($"Robot logístico {SerialNumber} descargó {actualUnloadWeight} kg. Carga restante: {CurrentPayloadWeightInKg} kg.");
    }
}