// Ruta: TuFabricaDDD.Persistence/RepositoryManager.cs
using TuFabricaDDD.Contracts.Repositories; // Para IRepositoryManager y todas las interfaces de repositorio
using TuFabricaDDD.Persistence.Contexts; // Para AppDbContext
using TuFabricaDDD.Persistence.Repositories; // Para las implementaciones concretas de repositorio
using System;
using System.Threading.Tasks;

namespace TuFabricaDDD.Persistence;

public class RepositoryManager : IRepositoryManager
{
    private readonly AppDbContext _context;

    // Campos privados para almacenar las instancias de los repositorios
    // Se inicializan de forma "lazy" (perezosa) para evitar crear instancias si no se usan.
    private IRobotRepository? _robotRepository;
    private IFixedRoboticArmRepository? _fixedRoboticArmRepository;
    private IHumanoidRepository? _humanoidRepository;
    private ILogisticsMobileRepository? _logisticsMobileRepository;
    private ICleaningMobileRepository? _cleaningMobileRepository;
    private IRobotStateChangeRecordRepository? _robotStateChangeRecordRepository;

    public RepositoryManager(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // Propiedades públicas que devuelven las instancias de los repositorios.
    // Usan el patrón de inicialización "lazy" (on-demand).
    public IRobotRepository Robots
    {
        get
        {
            // Si _robotRepository es null, lo inicializa; de lo contrario, devuelve la instancia existente.
            _robotRepository ??= new EfCoreRobotRepository(_context);
            return _robotRepository;
        }
    }

    public IFixedRoboticArmRepository FixedRoboticArms
    {
        get
        {
            _fixedRoboticArmRepository ??= new EfCoreFixedRoboticArmRepository(_context);
            return _fixedRoboticArmRepository;
        }
    }

    public IHumanoidRepository Humanoids
    {
        get
        {
            _humanoidRepository ??= new EfCoreHumanoidRepository(_context);
            return _humanoidRepository;
        }
    }

    public ILogisticsMobileRepository LogisticsMobiles
    {
        get
        {
            _logisticsMobileRepository ??= new EfCoreLogisticsMobileRepository(_context);
            return _logisticsMobileRepository;
        }
    }

    public ICleaningMobileRepository CleaningMobiles
    {
        get
        {
            _cleaningMobileRepository ??= new EfCoreCleaningMobileRepository(_context);
            return _cleaningMobileRepository;
        }
    }

    public IRobotStateChangeRecordRepository RobotStateChangeRecords
    {
        get
        {
            _robotStateChangeRecordRepository ??= new EfCoreRobotStateChangeRecordRepository(_context);
            return _robotStateChangeRecordRepository;
        }
    }

    // ¡Importante! SaveChangesAsync() NO va aquí, ya que lo movimos a IUnitOfWork.
    // Si tus servicios de aplicación necesitan guardar cambios, inyectarán IUnitOfWork por separado.
}