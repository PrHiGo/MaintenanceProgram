using MaintenanceProgram.Services;
using Microsoft.IdentityModel.Tokens;

var menu = new MenuService();

while (menu.startProgram)
{
    await menu.MainMenu();
}





