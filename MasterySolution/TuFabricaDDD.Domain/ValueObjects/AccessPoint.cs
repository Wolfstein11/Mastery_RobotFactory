using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Ruta: TuFabricaDDD.Domain/ValueObjects/AccessPoint.cs
using TuFabricaDDD.Domain.Common;
using System.Collections.Generic;

namespace TuFabricaDDD.Domain.ValueObjects;

/// <summary>
/// Representa un punto de acceso Wi-Fi en la red de la fábrica.
/// Es un objeto de valor inmutable.
/// </summary>
public sealed class AccessPoint : ValueObject
{
    public string Ssid { get; }
    public int Channel { get; } // Canal de la red Wi-Fi

    private AccessPoint() { } // Constructor privado para EF Core o serialización

    public AccessPoint(string ssid, int channel)
    {
        // Validaciones:
        // if (string.IsNullOrWhiteSpace(ssid)) throw new ArgumentException("SSID cannot be null or empty.", nameof(ssid));
        // if (channel < 1 || channel > 14) throw new ArgumentOutOfRangeException("Channel must be between 1 and 14.", nameof(channel));

        Ssid = ssid;
        Channel = channel;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Ssid;
        yield return Channel;
    }

    public override string ToString()
    {
        return $"SSID: {Ssid}, Channel: {Channel}";
    }
}
