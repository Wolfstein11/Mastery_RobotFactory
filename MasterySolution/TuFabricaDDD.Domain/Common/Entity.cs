// Ruta: TuFabricaDDD.Domain/Common/Entity.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace TuFabricaDDD.Domain.Common;

/// <summary>
/// Clase base abstracta para todas las entidades del dominio.
/// Una entidad se define por su identidad única (Id), no por sus atributos.
/// Hereda de CheckableObject para permitir la validación de reglas de negocio
/// utilizando el patrón Resultado (FluentResults).
/// </summary>
public abstract class Entity : CheckableObject,IEntity
{
    /// <summary>
    /// El identificador único de la entidad. Es de tipo Guid para asegurar unicidad global.
    /// El 'protected set' permite que el Id sea establecido durante la creación o
    /// por el ORM (Object-Relational Mapper) al cargar la entidad desde la persistencia,
    /// pero no permite cambios externos posteriores.
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Constructor protegido sin parámetros.
    /// Requerido por Entity Framework Core para la hidratación de entidades desde la base de datos.
    /// También se utiliza para generar un nuevo ID por defecto cuando una nueva entidad es creada
    /// por el dominio (por ejemplo, a través de un constructor de Robot que no reciba un ID).
    /// </summary>
    protected Entity()
    {
        Id = Guid.NewGuid(); // Genera un Id único al instanciar una nueva entidad
    }

    /// <summary>
    /// Constructor protegido. Las clases derivadas deben llamar a este constructor
    /// para inicializar la identidad de la entidad.
    /// Para una nueva entidad, se pasa Guid.NewGuid().
    /// Para una entidad cargada de persistencia, se pasa el Id existente.
    /// </summary>
    /// <param name="id">El identificador único (Guid) para la entidad.</param>
    /// <exception cref="ArgumentException">Se lanza si el Guid proporcionado es Guid.Empty.</exception>
    protected Entity(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("El ID de la entidad no puede ser un Guid vacío.", nameof(id));
        }
        Id = id;
    }

    /// <summary>
    /// Compara dos entidades por su identidad (Id).
    /// </summary>
    /// <param name="obj">El objeto a comparar.</param>
    /// <returns>True si los objetos son la misma entidad (mismo tipo y mismo Id), de lo contrario False.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Entity otherEntity)
        {
            return false;
        }

        // Si los objetos son la misma instancia de referencia, son iguales.
        if (ReferenceEquals(this, otherEntity))
        {
            return true;
        }

        // Si uno es nulo y el otro no, no son iguales.
        if (ReferenceEquals(null, otherEntity))
        {
            return false;
        }

        // Si el tipo de la instancia actual es diferente al tipo de la otra entidad,
        // no son consideradas la misma entidad. Esto ayuda a evitar comparar, por ejemplo,
        // un 'Robot' con un 'Automobile' aunque tuvieran el mismo Guid.
        // Si usas proxies de EF Core, GetType() podría devolver un tipo proxy,
        // en ese caso podrías usar 'IsAssignableFrom' o comparar el tipo base real.
        // Por simplicidad, GetType() es un buen punto de partida para DDD.
        if (GetType() != otherEntity.GetType())
        {
            return false;
        }

        // Dos entidades son iguales si tienen el mismo Id.
        return Id.Equals(otherEntity.Id);
    }

    /// <summary>
    /// Sobrecarga del operador de igualdad para comparar entidades.
    /// </summary>
    public static bool operator ==(Entity left, Entity right)
    {
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
        }
        return left.Equals(right);
    }

    /// <summary>
    /// Sobrecarga del operador de desigualdad para comparar entidades.
    /// </summary>
    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Calcula el código hash de la entidad basado en su Id.
    /// </summary>
    /// <returns>El código hash del Id de la entidad.</returns>
    public override int GetHashCode()
    {
        // Si el Id es Guid.Empty (ej. para una entidad recién creada antes de ser persistida),
        // el hash code será el de Guid.Empty.
        return Id.GetHashCode();
    }
}