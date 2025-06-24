// Ruta: TuFabricaDDD.Contracts/Repositories/IRepositoryManager.cs
// ¡Nota! Ya no incluye 'using System.Threading.Tasks;' porque SaveChangesAsync() se ha movido

namespace TuFabricaDDD.Contracts.Repositories;

/// <summary>
/// Define el contrato para el Administrador de Repositorios.
/// Su responsabilidad es proporcionar acceso a todos los repositorios específicos del dominio.
/// </summary>
public interface IRepositoryManager
{
    // Repositorios de Entidades de Robot (Agregados Raíz)
    IRobotRepository Robots { get; }
    IFixedRoboticArmRepository FixedRoboticArms { get; }
    IHumanoidRepository Humanoids { get; }
    ILogisticsMobileRepository LogisticsMobiles { get; }
    ICleaningMobileRepository CleaningMobiles { get; }

    // Repositorio para el registro de cambios de estado
    IRobotStateChangeRecordRepository RobotStateChangeRecords { get; }

    // ¡Importante! El método SaveChangesAsync() se ha movido a IUnitOfWork
    // No lo incluyas aquí.
}