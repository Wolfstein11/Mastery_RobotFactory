// Ruta: TuFabricaDDD.Persistence/AppDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design; // Necesario para IDesignTimeDbContextFactory
using TuFabricaDDD.Persistence.Contexts; // Para AppDbContext

namespace TuFabricaDDD.Persistence.Contexts;

/// <summary>
/// Fábrica en tiempo de diseño para AppDbContext.
/// Permite que las herramientas de Entity Framework Core (dotnet ef)
/// creen una instancia de AppDbContext cuando se ejecutan comandos de migración.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Aquí debes proporcionar una cadena de conexión ficticia o una real
        // que las herramientas de EF Core puedan usar para conectarse a la base de datos
        // y generar/aplicar migraciones. NO DEBE ser la cadena de conexión de producción.
        // Puedes obtenerla de alguna configuración o simplemente hardcodear una para diseño/desarrollo local.
        // Por ejemplo, para PostgreSQL local:
        string connectionString = "Host=localhost;Port=5432;Database=TuFabricaDb_Dev;Username=postgres;Password=qwerty";

        // IMPORTANTE: Reemplaza "tu_password_aqui" con tu contraseña real de PostgreSQL.
        // Considera usar variables de entorno o user-secrets para datos sensibles.
        // En una aplicación real, no harías esto en producción, solo para el entorno de desarrollo/migraciones.

        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}