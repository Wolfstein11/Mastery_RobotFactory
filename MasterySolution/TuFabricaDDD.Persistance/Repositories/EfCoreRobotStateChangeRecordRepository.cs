// Ruta: TuFabricaDDD.Persistence/Repositories/EfCoreRobotStateChangeRecordRepository.cs
using Microsoft.EntityFrameworkCore;
using TuFabricaDDD.Contracts.Repositories; // Para IRobotStateChangeRecordRepository
using TuFabricaDDD.Domain.Records; // Para RobotStateChangeRecord
using TuFabricaDDD.Persistence.Contexts; // Para AppDbContext
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TuFabricaDDD.Persistence.Repositories;

public class EfCoreRobotStateChangeRecordRepository : IRobotStateChangeRecordRepository
{
    private readonly AppDbContext _context;

    public EfCoreRobotStateChangeRecordRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(RobotStateChangeRecord record)
    {
        if (record == null) throw new ArgumentNullException(nameof(record));
        await _context.RobotStateChangeRecords.AddAsync(record);
    }

    public async Task<IEnumerable<RobotStateChangeRecord>> GetByRobotIdAsync(Guid robotId)
    {
        return await _context.RobotStateChangeRecords
                             .Where(r => r.RobotId == robotId)
                             .OrderBy(r => r.OccurringTime) // Ordenar por tiempo para ver la secuencia
                             .ToListAsync();
    }

    public async Task<RobotStateChangeRecord?> GetByRobotIdAndOccurringTimeAsync(Guid robotId, DateTime occurringTime)
    {
        return await _context.RobotStateChangeRecords
                             .SingleOrDefaultAsync(r => r.RobotId == robotId && r.OccurringTime == occurringTime);
    }
}