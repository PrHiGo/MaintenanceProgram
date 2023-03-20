using System.Linq.Expressions;
using MaintenanceProgram.Contexts;
using MaintenanceProgram.Models.Entities;
using MaintenanceProgram.Models.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MaintenanceProgram.Services;

internal class TicketService: GenericService<TicketEntity>
{
    private readonly DataContext _context = new DataContext();
    private readonly UserEntity _user = new UserEntity();

    public override async Task<IEnumerable<TicketEntity>> GetAllAsync()
    {
        return await _context.Tickets.Include(x => x.User).Include(x => x.StatusType).ToListAsync();
    }

    public override async Task<TicketEntity> GetSingleAsync(Expression<Func<TicketEntity, bool>> predicate)
    {
        var item = await _context.Tickets
            .Include(x => x.User).ThenInclude(x => x.Address)
            .Include(x => x.StatusType)
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(predicate);

        if (item != null)
        {
            return item;
        }

        return null!;
    }

    //public async Task<TicketEntity> DeleteTicketAsync(int ticketId)
    //{
    //    var ticketEntity = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == ticketId);
    //    if (ticketEntity != null)
    //    {
    //        return ticketEntity;
    //    }

    //    return null!;
    //}

    public async Task<TicketEntity> CreateTicketAsync(TicketRegistrationForm form)
    {
        var currentTime = DateTime.Now;

        var ticketEntity = new TicketEntity()
        {
            Created = currentTime,
            Modified = currentTime,
            Title = form.Title,
            Description = form.Description,
            User = new UserEntity()
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                PhoneNumber = form.PhoneNumber,
            }
        };


        var statusTypeEntity = await _context.StatusTypes.FirstOrDefaultAsync(x => x.StatusName == "New");
        if (statusTypeEntity != null)
        {
            ticketEntity.StatusTypeId = statusTypeEntity.Id;
        }
        else
        {
            ticketEntity.StatusType = new StatusTypeEntity()
            {
                StatusName = "New"
            };
        }

        _context.Add(ticketEntity);
        await _context.SaveChangesAsync();
        return ticketEntity;
    }
}

