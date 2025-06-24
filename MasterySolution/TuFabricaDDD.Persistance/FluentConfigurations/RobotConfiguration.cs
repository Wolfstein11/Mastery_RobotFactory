// Ruta: TuFabricaDDD.Persistence/FluentConfigurations/RobotConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TuFabricaDDD.Domain.Entities; // Para Robot y MobileRobot
using TuFabricaDDD.Persistence.FluentConfigurations.Common; // Para EntityTypeConfigurationBase

namespace TuFabricaDDD.Persistence.FluentConfigurations;

/// <summary>
/// Configuración Fluent API para la entidad base Robot.
/// Incluye propiedades comunes a todos los tipos de robots y sus Value Objects.
/// </summary>
public class RobotConfiguration : EntityTypeConfigurationBase<Robot>
{
    public override void Configure(EntityTypeBuilder<Robot> builder)
    {
        // Aplica la configuración base para la clave primaria (Id)
        base.Configure(builder);

        // ¡AÑADIR ESTA LÍNEA AQUÍ! La clave primaria del agregado raíz 'Robot'.
        builder.HasKey(r => r.Id); // Configura la clave primaria para la tabla Robots

        // Mapeo a la tabla principal para toda la jerarquía de herencia (TPH)
        builder.ToTable("Robots");

        // Propiedades de la clase Robot
        builder.Property(r => r.SerialNumber)
            .IsRequired()
            .HasMaxLength(50);

        // Índice único para SerialNumber
        builder.HasIndex(r => r.SerialNumber).IsUnique();

        // Mapeo de Enums a string (Category y Status)
        builder.Property(r => r.Category)
            .HasConversion<string>();

        builder.Property(r => r.Status)
            .HasConversion<string>();

        // Mapeo de Value Objects como tipos poseídos (Owned Types)
        builder.OwnsOne(r => r.CurrentLocation, location =>
        {
            location.Property(l => l.XCoordinate).HasColumnName("CurrentLocation_X");
            location.Property(l => l.YCoordinate).HasColumnName("CurrentLocation_Y");
        });

        builder.OwnsOne(r => r.NetworkLocation, networkLocation =>
        {
            networkLocation.Property(nl => nl.IpAddress).HasColumnName("NetworkLocation_IPAddress");
            //networkLocation.Property(nl => nl.ConnectedAccessPoint).HasColumnName("NetworkLocation_ConnectedAccessPoint");

            
            // =====================================================================
            // ¡¡¡AÑADIR ESTO PARA EL OWNED TYPE ANIDADO: AccessPoint!!!
            networkLocation.OwnsOne(nl => nl.ConnectedAccessPoint, accessPoint =>
            {
                // Mapea las propiedades de AccessPoint a columnas en la tabla 'Robots'
                // con prefijos para evitar colisiones de nombres.
                accessPoint.Property(ap => ap.Ssid).HasColumnName("NetworkLocation_AccessPoint_Ssid");
                accessPoint.Property(ap => ap.Channel).HasColumnName("NetworkLocation_AccessPoint_Bssid");

                // Si AccessPoint tuviera más propiedades (ej. Channel, SecurityType), las añadirías aquí:
                // accessPoint.Property(ap => ap.Channel).HasColumnName("NetworkLocation_AccessPoint_Channel");
            });
            // =====================================================================
            

        });

        // Configuración para propiedades de la clase MobileRobot (la clase abstracta intermedia)
        // Aunque MobileRobot no tiene una tabla propia, sus propiedades se mapean a la tabla Robots.
        // No necesitamos una clase MobileRobotConfiguration separada para esto.
        //========Linea quitada=============================
        //builder.Property(m => (m as MobileRobot).Locomotion).HasConversion<string>(); // Mapeo explícito para la propiedad de MobileRobot
        //==================================================    
                  
        // También puedes configurar otras propiedades de MobileRobot aquí, si las hubiera
        //builder.Property(m => (m as MobileRobot).BatteryLevelPercentage).IsRequired();
    }
}