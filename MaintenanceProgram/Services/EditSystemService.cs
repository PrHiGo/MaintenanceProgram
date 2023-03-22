using MaintenanceProgram.Models.Forms;

namespace MaintenanceProgram.Services;

internal class EditSystemService
{
    private readonly UserService _userService = new UserService();
    public async Task EditSystem()
    {
        Console.Clear();
        Console.WriteLine("***** Administration ******");
        Console.WriteLine("1. Skapa ny användare");
        Console.WriteLine("2. Visa användare");
        Console.WriteLine("3. Tillbaka");


        if (Int32.TryParse(Console.ReadLine(), out var option))
        {
            switch (option)
            {
                case 1:
                    await CreateUser();
                    break;

                case 2:
                    await ShowUser();
                    break;
                case 3:
                    break;
            }
        }
        else
        {
            Console.WriteLine("Vänligen ange en siffra");
        }
    }

    public async Task CreateUser()
    {
        var form = new UserForm();

        Console.Clear();
        Console.WriteLine("***** Ny Användare *****");

        Console.WriteLine("Namn: "); 
        form.FirstName = Console.ReadLine() ?? "";
        Console.WriteLine("Efternamn"); 
        form.LastName = Console.ReadLine() ?? "";
        Console.WriteLine("Email"); 
        form.Email = Console.ReadLine() ?? "";
        Console.WriteLine("Telefonnummer");
        form.PhoneNumber = Console.ReadLine() ?? "";

        await _userService.CreateSupport(form);
    }

    public async Task ShowUser()
    {
        Console.Clear();
        foreach (var user in await _userService.GetAllAsync())
        {
            Console.WriteLine($"{user.FirstName} {user.LastName} - {user.UserType}");
        }
        Console.ReadLine();
    }
}

