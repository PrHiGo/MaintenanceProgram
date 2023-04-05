using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using MaintenanceProgram.Contexts;
using MaintenanceProgram.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceProgram.Services;

internal class StatusTypeService : GenericService<StatusTypeEntity>
{
    private readonly DataContext _context = new DataContext();


    public async Task<StatusTypeEntity> UpdateStatus(string statusName)
    {
        var statusEntity = await _context.StatusTypes.FirstOrDefaultAsync(x => x.StatusName == statusName);
        if (statusEntity != null)
        {
            statusEntity.StatusName = statusName;
        }
        else
        {
            return null!;
        }

        _context.Add(statusEntity);
        await _context.SaveChangesAsync();
        return statusEntity;
    }

}


