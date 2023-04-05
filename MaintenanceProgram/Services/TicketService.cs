using System.Linq.Expressions;
using MaintenanceProgram.Contexts;
using MaintenanceProgram.Models.Entities;
using MaintenanceProgram.Models.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MaintenanceProgram.Services;

internal class TicketService : GenericService<TicketEntity>
{
    private readonly DataContext _context = new DataContext();
    private readonly UserEntity _user = new UserEntity();
    private readonly UserTypeEntity _userType = new UserTypeEntity();

    public override async Task<IEnumerable<TicketEntity>> GetAllAsync()
    {
        return await _context.Tickets.Include(x => x.User).Include(x => x.StatusType).ToListAsync();
    }

    public override async Task<TicketEntity> GetSingleAsync(Expression<Func<TicketEntity, bool>> predicate)
    {
        var item = await _context.Tickets
            .Include(x => x.User).ThenInclude(x => x.UserType)
            .Include(x => x.StatusType)
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(predicate);

        if (item != null)
        {
            return item;
        }

        return null!;
    }

    public async Task<TicketEntity> DeleteTicketAsync(Guid ticketId)
    {
        var ticketEntity = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == ticketId);
        if (ticketEntity != null)
        {
            _context.Remove(ticketEntity);
            await _context.SaveChangesAsync();
        }

        return null!;
    }

    public async Task<TicketEntity> CreateTicketEntityAsync(TicketRegistrationForm form)
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

        var userTypeEntity = await _context.UserTypes.FirstOrDefaultAsync(x => x.TypeName == "Kund");
        if (userTypeEntity != null)
        {
            ticketEntity.User.UserTypeId = userTypeEntity.Id;
        }
        else
        {
            ticketEntity.User.UserType = new UserTypeEntity()
            {
                TypeName = "Kund"
            };
        }

        var statusTypeEntity = await _context.StatusTypes.FirstOrDefaultAsync(x => x.StatusName == "Ej Påbörjad");
        if (statusTypeEntity != null)
        {
            ticketEntity.StatusTypeId = statusTypeEntity.Id;
        }
        else
        {
            ticketEntity.StatusType = new StatusTypeEntity()
            {
                StatusName = "Ej Påbörjad"
            };
        }

        _context.Add(ticketEntity);
        await _context.SaveChangesAsync();
        return ticketEntity;
    }

    public async Task NewTicketAsync()
    {
        var form = new TicketRegistrationForm();

        Console.Clear();
        Console.WriteLine("***** Skapa nytt ärende *****");

        Console.WriteLine("Titel:");
        form.Title = Console.ReadLine() ?? "";
        Console.WriteLine("Beskrivning:");
        form.Description = Console.ReadLine() ?? "";

        Console.WriteLine("\n ****Kontaktuppgifter****");

        Console.WriteLine("Förnamn");
        form.FirstName = Console.ReadLine() ?? "";
        Console.WriteLine("Efternamn:");
        form.LastName = Console.ReadLine() ?? "";
        Console.WriteLine("Email:");
        form.Email = Console.ReadLine() ?? "";
        Console.WriteLine("Telefonnummer:");
        form.PhoneNumber = Console.ReadLine() ?? "";

        await CreateTicketEntityAsync(form);

        Console.WriteLine($"Ärende {form.Title} har blivit skapat");
        Thread.Sleep(3000);
    }

    public async Task ShowTicketsAsync()
    {
        Console.Clear();
        foreach (var tickets in await GetAllAsync())
        {
            Console.WriteLine(
                $"ID: {tickets.Id} - {tickets.Title} - {tickets.Created} - {tickets.StatusType.StatusName}");
        }

        Console.WriteLine("\n ***** Skriv in ID på det ärende du vill hantera *****");
        try
        {
            var result = await GetSingleAsync(x => x.Id == Guid.Parse(Console.ReadLine() ?? ""));
            Console.Clear();

            Console.WriteLine($"{result.Id}");
            Console.WriteLine($"Titel: {result.Title}");
            Console.WriteLine($"Skapad: {result.Created}");
            Console.WriteLine($"Status: {result.StatusType.StatusName}");
            Console.WriteLine($"Beskrivning: " +
                              $"\n {result.Description}");
            Console.WriteLine($"Senast ändrad: {result.Modified}");


            await EditTicketAsync(result.Id);
        }
        catch
        {

            Console.WriteLine("Inget ärende med angivet ID hittades");
        }

    }

    public async Task EditTicketAsync(Guid ticketId)
    {
        Console.WriteLine("\n1. Ta bort ärende");
        Console.WriteLine($"2. Uppdatera status");


        Int32.TryParse(Console.ReadLine(), out var option);
        switch (option)
        {
            case 1:
                await DeleteTicketAsync(ticketId);
                break;
            case 2:
                await ChangeStatus();
                break;
        }
        async Task ChangeStatus()
        {
            string notStarted = "Ej Påbörjad";
            string pending = "Pågående";
            string done = "Avslutad";

            Console.WriteLine("\n1. Ej Påbörjad");
            Console.WriteLine($"2. Pågående");
            Console.WriteLine($"3. Avslutad");

            Int32.TryParse(Console.ReadLine(), out var option);
            switch (option)
            {
                case 1:
                    await SetStatusTo(notStarted);
                    break;
                case 2:
                    await SetStatusTo(pending);
                    break;
                case 3:
                    await SetStatusTo(done);
                    break;
            }

            async Task SetStatusTo(string newStatus)
            {
                var result = await GetSingleAsync(x => x.Id == ticketId);

                if (result != null!)
                {
                    if (!string.IsNullOrEmpty(result.User.UserType.TypeName))
                        result.User.UserType.TypeName = newStatus;
                }

                var statusTypeEntity =
                    await _context.StatusTypes.FirstOrDefaultAsync(x => x.StatusName == newStatus);
                if (statusTypeEntity != null)
                {
                    result.StatusTypeId = statusTypeEntity.Id;
                }
                else
                {
                    result.StatusType = new StatusTypeEntity()
                    {
                        StatusName = newStatus
                    };
                }

                _context.StatusTypes.Update(result.StatusType);
                _context.Entry(result).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                Console.WriteLine($"Status ändrad till {newStatus}");
                Thread.Sleep(2000);
            }
        }

    }
}

