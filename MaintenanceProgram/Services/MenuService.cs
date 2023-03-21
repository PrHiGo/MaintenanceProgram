using MaintenanceProgram.Models.Forms;
using Newtonsoft.Json;

namespace MaintenanceProgram.Services;

internal class MenuService
{
    #region variables
    private readonly TicketService _ticketService = new TicketService();
    private readonly EditSystemService _editSystemService = new EditSystemService();
    public bool startProgram { get; set; } = true;
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
                    await CreateTicket();
                    break;

                case 2:
                    await ShowTickets();
                    break;
                case 3:
                    await _editSystemService.EditSystem();
                    break;
                case 4:
                    startProgram = false;
                    break;
            }
        }
        else
        {
            Console.WriteLine("Vänligen ange en siffra");
        }

    }

    public async Task CreateTicket()
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

        await _ticketService.CreateTicketAsync(form);

        Console.WriteLine($"Ärende {form.Title} har blivit skapat");
        Thread.Sleep(3000);
    }

    public async Task ShowTickets()
    {
        Console.Clear();
        foreach (var tickets in await _ticketService.GetAllAsync())
        {
            Console.WriteLine($"{tickets.Id} - {tickets.Title} - {tickets.Created} - {tickets.StatusType.StatusName}");
        }

        Console.WriteLine("\n ***** Skriv in ID på det ärende du vill hantera *****");
        try
        {
            var result = await _ticketService.GetSingleAsync(x => x.Id == Guid.Parse(Console.ReadLine() ?? ""));
            Console.Clear();
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
        catch
        {

            Console.WriteLine("Inget ärende med angivet ID hittades");
        }

        Console.ReadLine();
    }
}

