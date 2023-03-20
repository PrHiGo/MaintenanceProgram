using MaintenanceProgram.Models.Forms;

namespace MaintenanceProgram.Services;

internal class MenuService
{
    private readonly TicketService _ticketService = new TicketService();
    private readonly UserService _userService = new UserService();
    public async Task MainMenu()
    {

        Console.Clear();
        Console.WriteLine("*****Huvudmeny*****");
        Console.WriteLine("1. Skapa ett nytt ärende");
        Console.WriteLine("2. Visa ärenden");
        Console.WriteLine("3. Administrera användare");
        Console.WriteLine("4. Avsluta programmet");

        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                await CreateTicket();
                break;

            case "2":
                await ShowTickets();
                break;

            case "3":
                await EditUsers();
                break;

            case "4":

                break;
        }
    }

    public async Task CreateTicket()
    {
        var form = new TicketRegistrationForm();
        Console.Clear();
        Console.WriteLine("****Skapa Ärende****");
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
        Console.ReadKey();
    }

    public async Task ShowTickets()
    {
        foreach (var tickets in await _ticketService.GetAllAsync())
        {
            Console.WriteLine($"{tickets.Id} - {tickets.Title} - {tickets.Created} - {tickets.StatusType.StatusName}");
        }

        Console.ReadLine();

    }

    public async Task EditUsers()
    {
        await _userService.GetAllAsync();
        Console.ReadKey();

    }

}

