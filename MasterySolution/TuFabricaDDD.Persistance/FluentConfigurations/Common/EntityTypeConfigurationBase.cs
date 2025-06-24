// Ruta: TuFabricaDDD.Persistence/FluentConfigurations/Common/EntityTypeConfigurationBase.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Security.Principal;
using TuFabricaDDD.Domain.Common; // Necesario para IEntity (si lo usamos para todas las entidades)

namespace TuFabricaDDD.Persistence.FluentConfigurations.Common;

/// <summary>
/// Clase base abstracta para configuraciones de entidades de Entity Framework Core usando Fluent API.
/// Proporciona configuraciones comunes aplicables a todas las entidades que heredan de IEntity.
/// </summary>
/// <typeparam name="TEntity">El tipo de entidad que se está configurando.</typeparam>
public abstract class EntityTypeConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IEntity // Restringe TEntity a ser una clase y que implemente IEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // Configuración de la clave primaria para todas las entidades que implementan IEntity
        // Asume que todas las entidades tienen una propiedad 'Id' de tipo Guid
        //builder.HasKey(e => e.Id);

        // Si tienes alguna propiedad común a TODAS las entidades (como CreatedDate, LastModifiedDate, etc.)
        // podrías configurarlas aquí. Por ahora, nos centramos solo en la clave primaria.
    }
}