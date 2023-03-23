using MaintenanceProgram.Contexts;
using MaintenanceProgram.Models.Entities;
using MaintenanceProgram.Models.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace MaintenanceProgram.Services;

internal class MenuService
{
    #region variables
    private readonly TicketService _ticketService = new TicketService();
    private readonly EditSystemService _editSystemService = new EditSystemService();

    public bool StartProgram { get; set; } = true;

    #endregion

    public async Task MainMenu()
    {
        Console.Clear();
        Console.WriteLine("***** Huvudmeny *****");
        Console.WriteLine("1. Skapa ett nytt ärende");
        Console.WriteLine("2. Visa ärenden");
        Console.WriteLine("3. Administrera användare");
        Console.WriteLine("4. Avsluta");

        if (Int32.TryParse(Console.ReadLine(), out var option))
        {
            switch (option)
            {
                case 1:
                    await _ticketService.NewTicketAsync();
                    break;

                case 2:
                    await _ticketService.ShowTicketsAsync();
                    break;
                case 3:
                    await _editSystemService.EditUserAsync();
                    break;
                case 4:
                    StartProgram = false;
                    break;
            }
        }
        else
        {
            Console.WriteLine("Vänligen ange en siffra");
        }
    }
}

