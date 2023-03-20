using System.Linq.Expressions;
using MaintenanceProgram.Contexts;
using MaintenanceProgram.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceProgram.Services
{
    internal class LoginService : GenericService<UserEntity>
    {
        private readonly DataContext _context = new DataContext();


        public override async Task<UserEntity> GetSingleAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            var user = await _context.Users.Include(x => x.Email).FirstOrDefaultAsync(predicate);

            if (user != null)
            {
                return user;
            }

            return null!;
        }
    }
}
