using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Ruta: TuFabricaDDD.Domain/ValueObjects/Location.cs
using TuFabricaDDD.Domain.Common;
using System.Collections.Generic;

namespace TuFabricaDDD.Domain.ValueObjects;

/// <summary>
/// Representa la ubicación física de una unidad robótica dentro de la fábrica.
/// Es un objeto de valor inmutable.
/// </summary>
public sealed class Location : ValueObject
{
    public string AreaName { get; }
    public double XCoordinate { get; }
    public double YCoordinate { get; }
    public double ZCoordinate { get; } // Consideramos un entorno 3D para la fábrica

    private Location() { } // Constructor privado para EF Core o serialización

    public Location(string areaName, double xCoordinate, double yCoordinate, double zCoordinate = 0.0)
    {
        // Puedes añadir validaciones aquí, por ejemplo:
        // if (string.IsNullOrWhiteSpace(areaName)) throw new ArgumentException("Area name cannot be null or empty.", nameof(areaName));
        // if (xCoordinate < 0 || yCoordinate < 0 || zCoordinate < 0) throw new ArgumentOutOfRangeException("Coordinates cannot be negative.");

        AreaName = areaName;
        XCoordinate = xCoordinate;
        YCoordinate = yCoordinate;
        ZCoordinate = zCoordinate;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return AreaName;
        yield return XCoordinate;
        yield return YCoordinate;
        yield return ZCoordinate;
    }

    public override string ToString()
    {
        return $"{AreaName} ({XCoordinate}, {YCoordinate}, {ZCoordinate})";
    }
}
