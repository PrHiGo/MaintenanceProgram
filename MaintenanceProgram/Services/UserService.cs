using MaintenanceProgram.Contexts;
using MaintenanceProgram.Models.Entities;
using MaintenanceProgram.Models.Forms;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceProgram.Services;

internal class UserService : GenericService<UserEntity>
{
    private readonly DataContext _context = new DataContext();

    public async Task<UserEntity> CreateSupport(UserForm form)
    {
        var userEntity = new UserEntity()
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            Email = form.Email,
            PhoneNumber = form.PhoneNumber
        };

        var userTypeEntity = await _context.UserTypes.FirstOrDefaultAsync(x => x.TypeName == "Support");
        if (userTypeEntity != null)
        {
            userEntity.UserTypeId = userTypeEntity.Id;
        }
        else
        {
            userEntity.UserType = new UserTypeEntity()
            {
                TypeName = "Support"
            };
        }

        _context.Add(userEntity);
        await _context.SaveChangesAsync();
        return userEntity;
    }

}
