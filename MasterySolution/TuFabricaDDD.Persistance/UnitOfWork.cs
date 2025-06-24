// Ruta: TuFabricaDDD.Persistence/UnitOfWork.cs
using TuFabricaDDD.Contracts.Repositories; // Para IUnitOfWork
using TuFabricaDDD.Persistence.Contexts; // Para AppDbContext
using System.Threading.Tasks;

namespace TuFabricaDDD.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Guarda todos los cambios pendientes en el contexto de la base de datos.
    /// </summary>
    /// <returns>El número de estados de entidad escritos en la base de datos.</returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    // Si IUnitOfWork implementara IDisposable, podrías implementar Dispose aquí.
    // Sin embargo, para la mayoría de los escenarios de ASP.NET Core, el DbContext
    // ya es gestionado por el contenedor de DI y se dispone automáticamente.
}