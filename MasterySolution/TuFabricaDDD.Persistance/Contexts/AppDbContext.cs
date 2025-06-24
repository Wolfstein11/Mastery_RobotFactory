// Ruta: TuFabricaDDD.Persistence/Contexts/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using TuFabricaDDD.Domain.Entities; // Para acceder a tus entidades de dominio (Robot, FixedRoboticArm, etc.)
using TuFabricaDDD.Domain.Records; // Para acceder a RobotStateChangeRecord
using System.Reflection; // Necesario para cargar las configuraciones de Fluent API
using System; // Necesario para Guid

namespace TuFabricaDDD.Persistence.Contexts;

public class AppDbContext : DbContext
{
    // Constructor principal que acepta DbContextOptions, lo que permite configurar el contexto
    // desde el exterior (ej. desde tu proyecto de API o de inicio) para la cadena de conexión, etc.
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // --- DbSets para tus Entidades ---
    public DbSet<Robot> Robots { get; set; } // Representa la tabla base para todos los robots
    public DbSet<FixedRoboticArm> FixedRoboticArms { get; set; }
    public DbSet<Humanoid> Humanoids { get; set; }
    public DbSet<LogisticsMobile> LogisticsMobiles { get; set; }
    public DbSet<CleaningMobile> CleaningMobiles { get; set; }

    // --- DbSet para RobotStateChangeRecord ---
    // Utiliza la sintaxis de expresión para el getter, más conciso.
    public DbSet<RobotStateChangeRecord> RobotStateChangeRecords => Set<RobotStateChangeRecord>();

    // --- OnConfiguring: Para configurar el proveedor de base de datos directamente si no se hace externamente ---
    // Esta sección se usa a menudo para configuraciones básicas o cuando no se usa la inyección de dependencias
    // para pasar DbContextOptions. Si se usa la inyección de dependencias (recomendado para ASP.NET Core),
    // esta configuración puede sobrescribirse o ser redundante.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // Si no se han configurado opciones de base de datos externamente, usa Npgsql por defecto.
        // Se recomienda obtener la cadena de conexión de una configuración (ej. appsettings.json)
        // y pasarla aquí, o configurar el DbContext en tu startup (Program.cs/Startup.cs).
        // Ejemplo: optionsBuilder.UseNpgsql("Host=localhost;Database=tu_db;Username=tu_user;Password=tu_password;");
        optionsBuilder.UseNpgsql();
    }

    // --- OnModelCreating: Configuración del modelo de base de datos con Fluent API ---
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Esta línea es suficiente para aplicar todas las configuraciones definidas
        // en clases que implementan IEntityTypeConfiguration<TEntity> dentro de este ensamblado.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Opcional: Si quieres personalizar el nombre de la columna discriminadora, puedes hacerlo aquí
        // modelBuilder.Entity<Robot>()
        //     .HasDiscriminator<string>("RobotType")
        //     .HasValue<FixedRoboticArm>("FixedArm")
        //     .HasValue<Humanoid>("Humanoid")
        //     .HasValue<LogisticsMobile>("Logistics")
        //     .HasValue<CleaningMobile>("Cleaning");

        // Ya no necesitas las configuraciones repetidas aquí
        // ya que están en RobotConfiguration.cs y RobotStateChangeRecordConfiguration.cs:
        // modelBuilder.Entity<Robot>().HasKey(r => r.Id);
        // modelBuilder.Entity<Robot>().Property(r => r.SerialNumber).IsRequired().HasMaxLength(50);
        // modelBuilder.Entity<Robot>().HasIndex(r => r.SerialNumber).IsUnique();
        // ... y el mapeo de enums y Owned Types para Robot
        // ... y el mapeo para RobotStateChangeRecord
    }

    #region Helpers

    /// <summary>
    /// Crea una instancia de DbContextOptions para AppDbContext usando una cadena de conexión Npgsql.
    /// Útil para pruebas o escenarios donde el DbContextOptions se necesita programáticamente.
    /// </summary>
    /// <param name="connectionString">La cadena de conexión a la base de datos PostgreSQL.</param>
    /// <returns>DbContextOptions configurado.</returns>
    private static DbContextOptions GetOptions(string connectionString)
    {
        // Esto crea un DbContextOptionsBuilder, le aplica la configuración Npgsql, y luego obtiene las opciones.
        return NpgsqlDbContextOptionsBuilderExtensions.UseNpgsql(new DbContextOptionsBuilder<AppDbContext>(), connectionString).Options;
    }

    #endregion
}