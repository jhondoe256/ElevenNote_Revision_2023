using ElevenNote.Data;
using ElevenNote.Data.AppContext;
using ElevenNote.Models.User;
using Microsoft.EntityFrameworkCore;

namespace ElevenNote.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUserAsync(UserRegister model)
        {
            if(await GetUserByEmailAsync(model.Email) !=null || await GetUserByUserNameAsync(model.UserName)!=null)
            return false;
            
            UserEntity entity = new()
            {
                Email = model.Email,
                UserName = model.UserName,
                Pasword = model.Password,
                DateCreated = DateTime.UtcNow
            };

            await _context.Users.AddAsync(entity);
            int numberOfChanges = await _context.SaveChangesAsync();
            return numberOfChanges == 1;
        }

        private async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user=>user.Email.ToLower() == email.ToLower());
        }

        private async Task<UserEntity?> GetUserByUserNameAsync(string UserName)
        {
            return await _context.Users.FirstOrDefaultAsync(user=>user.UserName.ToLower() == UserName.ToLower());
        }
    }
}