using MaintenanceProgram.Services;
using Microsoft.IdentityModel.Tokens;

var menu = new MenuService();

while (menu.StartProgram)
{
    await menu.MainMenu();
}





