using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Ruta: TuFabricaDDD.Domain/ValueObjects/NetworkLocation.cs
using TuFabricaDDD.Domain.Common;
using System.Net; // Para IPAddress

namespace TuFabricaDDD.Domain.ValueObjects;

/// <summary>
/// Representa la ubicación de una unidad robótica dentro de la red inalámbrica de la fábrica.
/// Es un objeto de valor inmutable.
/// </summary>
public sealed class NetworkLocation : ValueObject
{
    public IPAddress IpAddress { get; } // Usamos System.Net.IPAddress para tipo fuerte
    public AccessPoint ConnectedAccessPoint { get; }

    private NetworkLocation() { } // Constructor privado para EF Core o serialización

    public NetworkLocation(string ipAddress, AccessPoint connectedAccessPoint)
    {
        // Validaciones:
        if (!IPAddress.TryParse(ipAddress, out var parsedIpAddress))
        {
            throw new ArgumentException("Invalid IP address format.", nameof(ipAddress));
        }
        if (connectedAccessPoint == null)
        {
            throw new ArgumentNullException(nameof(connectedAccessPoint), "Connected access point cannot be null.");
        }

        IpAddress = parsedIpAddress;
        ConnectedAccessPoint = connectedAccessPoint;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IpAddress;
        yield return ConnectedAccessPoint;
    }

    public override string ToString()
    {
        return $"IP: {IpAddress}, Connected to: {ConnectedAccessPoint}";
    }
}
