using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Ruta: TuFabricaDDD.Domain/Common/ValueObject.cs
namespace TuFabricaDDD.Domain.Common;

/// <summary>
/// Clase base abstracta para implementar el patrón Value Object.
/// Proporciona la lógica para la igualdad estructural de objetos de valor.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Obtiene una colección de objetos que representan los componentes que definen la igualdad de este Value Object.
    /// Las propiedades y campos que se utilizan para determinar si dos Value Objects son iguales deben ser retornados aquí.
    /// </summary>
    /// <returns>Una colección de objetos que definen la igualdad.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
        {
            return true;
        }

        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }
}
