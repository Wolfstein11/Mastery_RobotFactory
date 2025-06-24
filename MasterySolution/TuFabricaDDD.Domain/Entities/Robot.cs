// Ruta: TuFabricaDDD.Domain/Entities/Robot.cs
using TuFabricaDDD.Domain.Common; // ¡Asegúrate de que este using esté presente!
using TuFabricaDDD.Domain.ValueObjects;
using TuFabricaDDD.Domain.Types;
using TuFabricaDDD.Domain.Rules;
using TuFabricaDDD.Domain.Errors;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuFabricaDDD.Domain.Entities;

public abstract class Robot : Entity, IAggregateRoot // Aquí se implementa la interfaz
{
    public string SerialNumber { get; protected set; }
    public RobotCategory Category { get; protected set; }
    public Location CurrentLocation { get; protected set; }
    public NetworkLocation NetworkLocation { get; protected set; }
    public RobotStatus Status { get; protected set; }

    // Constructor privado para Entity Framework Core
    protected Robot() { }

    protected Robot(
        Guid id,
        string serialNumber,
        RobotCategory category,
        Location initialLocation,
        NetworkLocation initialNetworkLocation)
        : base(id)
    {
        // Los constructores deben lanzar excepciones que hereden de System.Exception.
        // Reutilizamos los mensajes de nuestros InvalidArgumentError.
        if (string.IsNullOrWhiteSpace(serialNumber))
            throw new ArgumentException(new InvalidArgumentError(nameof(serialNumber), "El número de serie no puede ser nulo o vacío.").Message, nameof(serialNumber));
        if (initialLocation == null)
            throw new ArgumentNullException(nameof(initialLocation), new InvalidArgumentError(nameof(initialLocation), "La ubicación inicial no puede ser nula.").Message);
        if (initialNetworkLocation == null)
            throw new ArgumentNullException(nameof(initialNetworkLocation), new InvalidArgumentError(nameof(initialNetworkLocation), "La ubicación de red inicial no puede ser nula.").Message);

        SerialNumber = serialNumber;
        Category = category;
        CurrentLocation = initialLocation;
        NetworkLocation = initialNetworkLocation;
        Status = RobotStatus.Idle; // Estado inicial por defecto
    }

    public Result ChangeStatus(RobotStatus newStatus)
    {
        if (Status == newStatus)
        {
            return Result.Ok().WithSuccess($"El robot {SerialNumber} ya se encuentra en el estado {newStatus}.");
        }

        Status = newStatus;
        return Result.Ok().WithSuccess($"El estado del robot {SerialNumber} ha cambiado a {newStatus}.");
    }

    public Result UpdateNetworkLocation(NetworkLocation newNetworkLocation)
    {
        if (newNetworkLocation == null)
        {
            return Result.Fail(new InvalidArgumentError(nameof(newNetworkLocation), "La nueva ubicación de red no puede ser nula."));
        }

        NetworkLocation = newNetworkLocation;
        return Result.Ok().WithSuccess($"La ubicación de red del robot {SerialNumber} ha sido actualizada a {newNetworkLocation.IpAddress}.");
    }

    public virtual Result UpdateLocation(Location newLocation)
    {
        if (newLocation == null)
        {
            return Result.Fail(new InvalidArgumentError(nameof(newLocation), "La nueva ubicación no puede ser nula."));
        }

        CurrentLocation = newLocation;
        return Result.Ok().WithSuccess($"La ubicación del robot {SerialNumber} ha sido actualizada a {newLocation.XCoordinate}, {newLocation.YCoordinate}.");
    }

    protected Result CheckRules(params IBusinessRule[] rules)
    {
        List<IError> errors = new List<IError>();
        foreach (var rule in rules)
        {
            if (rule.IsBroken())
            {
                errors.Add(new BusinessRuleError(rule.Message));
            }
        }

        if (errors.Any())
        {
            return Result.Fail(errors);
        }

        return Result.Ok();
    }
}