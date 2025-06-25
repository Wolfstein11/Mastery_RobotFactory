// Ruta: TuFabricaDDD.Persistence/FluentConfigurations/CleaningMobileConfiguration.cs
using Microsoft.EntityFrameworkCore; // Necesario para .HasConversion
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TuFabricaDDD.Domain.Entities; // Para CleaningMobile
using TuFabricaDDD.Persistence.FluentConfigurations.Common; // Para EntityTypeConfigurationBase
using Microsoft.EntityFrameworkCore.ChangeTracking; // Add this using

namespace TuFabricaDDD.Persistence.FluentConfigurations;

/// <summary>
/// Configuración Fluent API para la entidad CleaningMobile.
/// </summary>
public class CleaningMobileConfiguration : EntityTypeConfigurationBase<CleaningMobile>
{
    public override void Configure(EntityTypeBuilder<CleaningMobile> builder)
    {
        base.Configure(builder);

        // Configuración para la colección de strings 'EquippedCleaningTools'
        // Similar a Humanoid.EquippedTools, se serializa a una cadena.
        builder.Property(c => c.EquippedCleaningTools)
            .HasConversion(
                v => string.Join(";", v), // Convierte List<string> a string separado por ';'
                v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList() // Convierte string a List<string>
            )
            .HasMaxLength(1000)
            .Metadata.SetValueComparer(
                new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), 
                    c => c.ToList()
                )
            );
    }
}