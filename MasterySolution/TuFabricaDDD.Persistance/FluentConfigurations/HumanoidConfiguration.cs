// Ruta: TuFabricaDDD.Persistence/FluentConfigurations/HumanoidConfiguration.cs
using Microsoft.EntityFrameworkCore; // Necesario para .HasConversion
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TuFabricaDDD.Domain.Entities; // Para Humanoid
using TuFabricaDDD.Persistence.FluentConfigurations.Common; // Para EntityTypeConfigurationBase
using Microsoft.EntityFrameworkCore.ChangeTracking; // Add this using

namespace TuFabricaDDD.Persistence.FluentConfigurations;

/// <summary>
/// Configuración Fluent API para la entidad Humanoid.
/// </summary>
public class HumanoidConfiguration : EntityTypeConfigurationBase<Humanoid>
{
    public override void Configure(EntityTypeBuilder<Humanoid> builder)
    {
        base.Configure(builder);

        builder.Property(h => h.HasManipulators)
            .IsRequired();

        builder.Property(h => h.OperatingSystemVersion)
            .IsRequired()
            .HasMaxLength(50);

        // Configuración para la colección de strings 'EquippedTools'
        // EF Core no soporta colecciones de tipos primitivos directamente en la tabla principal.
        // Una opción común es serializarlos (ej. a JSON) o mapearlos a una tabla separada.
        // Por ahora, lo mapearemos como una cadena JSON para simplificar.
        builder.Property(h => h.EquippedTools)
            .HasConversion(
                v => string.Join(";", v), // Convierte List<string> a string separado por ';'
                v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList() // Convierte string a List<string>
            )
            // Define una longitud máxima para la columna en la DB
            // Considera usar un tipo de columna como 'jsonb' en PostgreSQL si quieres consultar dentro del JSON
            // builder.HasColumnType("jsonb"); // Requiere que Npgsql.EntityFrameworkCore.PostgreSQL lo soporte y PostgreSQL tenga el tipo.
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