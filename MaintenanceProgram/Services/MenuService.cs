using MaintenanceProgram.Models.Forms;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace MaintenanceProgram.Services;

internal class MenuService
{
    #region variables
    private readonly TicketService _ticketService = new TicketService();
    private readonly EditSystemService _editSystemService = new EditSystemService();
    private readonly CommentService _commentService = new CommentService();
    private readonly StatusTypeService _statusTypeService = new StatusTypeService();
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
                    await CreateTicketAsync();
                    break;

                case 2:
                    await ShowTicketsAsync();
                    break;
                case 3:
                    await _editSystemService.EditSystem(); // Lägg detta i en Metod föt att skapa användare
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

        async Task CreateTicketAsync()
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

        async Task ShowTicketsAsync()
        {
            Console.Clear();
            foreach (var tickets in await _ticketService.GetAllAsync())
            {
                Console.WriteLine($"ID: {tickets.Id} - {tickets.Title} - {tickets.Created} - {tickets.StatusType.StatusName}");
            }

            Console.WriteLine("\n ***** Skriv in ID på det ärende du vill hantera *****");
            try
            {
                var result = await _ticketService.GetSingleAsync(x => x.Id == Guid.Parse(Console.ReadLine() ?? ""));
                Console.Clear();

                Console.WriteLine($"{result.Id}");
                Console.WriteLine($"Titel: {result.Title}");
                Console.WriteLine($"Skapad: {result.Created}");
                Console.WriteLine($"Status: {result.StatusType.StatusName}");
                Console.WriteLine($"Beskrivning: " +
                                  $"\n {result.Description}");
                await GetComments();
                Console.WriteLine($"Senast ändrad: {result.Modified}");


                await EditTicketAsync(result.Id);
            }
            catch
            {

                Console.WriteLine("Inget ärende med angivet ID hittades");
            }

            async Task GetComments()
            {
                foreach (var comment in await _commentService.GetAllAsync())
                {
                    Console.WriteLine($"\nKommentarer: {comment.Comment}" +
                                      $"\nSkapad: {comment.Created}");
                }
            }

            async Task EditTicketAsync(Guid ticketId)
            {
                Console.WriteLine("\n1. Ta bort ärende");
                Console.WriteLine($"2. Lägg till kommentar");
                Console.WriteLine($"3. Uppdatera status");


                Int32.TryParse(Console.ReadLine(), out var option);
                switch (option)
                {
                    case 1:
                        await _ticketService.DeleteTicketAsync(ticketId);
                        break;
                    case 2:
                        
                        break;
                        await ChoseStatus();
                    case 3:
                        break;
                }

                async Task ChoseStatus()
                {
                    Console.WriteLine("1. Ny");
                    Console.WriteLine("2. Pågående");
                    Console.WriteLine("3. Klar");

                    Int32.TryParse(Console.ReadLine(), out var option);
                    switch (option)
                    {
                        case 1:
                            ;
                            break;
                        case 2:

                            break;
                            await ChoseStatus();
                        case 3:
                            break;
                    }
                }
            }
        }
    }
}

