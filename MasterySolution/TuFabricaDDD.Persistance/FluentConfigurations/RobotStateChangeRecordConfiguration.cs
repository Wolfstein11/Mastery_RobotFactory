// Ruta: TuFabricaDDD.Persistence/FluentConfigurations/RobotStateChangeRecordConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TuFabricaDDD.Domain.Records; // Para RobotStateChangeRecord

namespace TuFabricaDDD.Persistence.FluentConfigurations;

/// <summary>
/// Configuración Fluent API para el registro de cambios de estado del robot (RobotStateChangeRecord).
/// </summary>
public class RobotStateChangeRecordConfiguration : IEntityTypeConfiguration<RobotStateChangeRecord>
{
    public void Configure(EntityTypeBuilder<RobotStateChangeRecord> builder)
    {
        // Mapea el record a su propia tabla
        builder.ToTable("RobotStateChangeRecords");

        // Define la clave primaria compuesta
        builder.HasKey(r => new { r.RobotId, r.OccurringTime });

        // Propiedades obligatorias
        builder.Property(r => r.OccurringTime).IsRequired();
        builder.Property(r => r.OldStatus).HasConversion<string>().IsRequired();
        builder.Property(r => r.NewStatus).HasConversion<string>().IsRequired();

        // Propiedad opcional con longitud máxima
        builder.Property(r => r.Reason).HasMaxLength(255);
    }
}