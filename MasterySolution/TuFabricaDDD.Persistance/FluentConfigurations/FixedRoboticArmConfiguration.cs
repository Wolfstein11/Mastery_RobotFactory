// Ruta: TuFabricaDDD.Persistence/FluentConfigurations/FixedRoboticArmConfiguration.cs
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TuFabricaDDD.Domain.Entities; // Para FixedRoboticArm
using TuFabricaDDD.Persistence.FluentConfigurations.Common; // Para EntityTypeConfigurationBase

namespace TuFabricaDDD.Persistence.FluentConfigurations;

/// <summary>
/// Configuración Fluent API para la entidad FixedRoboticArm.
/// </summary>
public class FixedRoboticArmConfiguration : EntityTypeConfigurationBase<FixedRoboticArm>
{
    public override void Configure(EntityTypeBuilder<FixedRoboticArm> builder)
    {
        // Llama a la configuración base para Id y otras propiedades comunes de Entity
        base.Configure(builder);

        // Mapeo específico de propiedades de FixedRoboticArm
        builder.Property(f => f.ArmLengthInMeters)
            .IsRequired(); // Esta propiedad es obligatoria

        builder.Property(f => f.PayloadCapacityInKg)
            .IsRequired(); // Esta propiedad es obligatoria

        builder.Property(f => f.MountingPointId)
            .IsRequired() // Esta propiedad es obligatoria
            .HasMaxLength(100); // Establece una longitud máxima para la cadena
    }
}