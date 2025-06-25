// Ruta: TuFabricaDDD.Persistence/FluentConfigurations/LogisticsMobileConfiguration.cs
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TuFabricaDDD.Domain.Entities; // Para LogisticsMobile
using TuFabricaDDD.Persistence.FluentConfigurations.Common; // Para EntityTypeConfigurationBase

namespace TuFabricaDDD.Persistence.FluentConfigurations;

/// <summary>
/// Configuración Fluent API para la entidad LogisticsMobile.
/// </summary>
public class LogisticsMobileConfiguration : EntityTypeConfigurationBase<LogisticsMobile>
{
    public override void Configure(EntityTypeBuilder<LogisticsMobile> builder)
    {
        base.Configure(builder);

        builder.Property(l => l.MaxPayloadCapacityInKg)
            .IsRequired();
    }
}