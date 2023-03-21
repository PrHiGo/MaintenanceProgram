using Newtonsoft.Json;

namespace MaintenanceProgram.Services;

internal class ShowTicketService
{
    private readonly TicketService _ticketService = new TicketService();

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

