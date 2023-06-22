using ElevenNote.Data;
using ElevenNote.Data.AppContext;
using ElevenNote.Models.User;
using Microsoft.AspNetCore.Identity;
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

        public async Task<UserDetail> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null) return new UserDetail();

            return new UserDetail
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateCreated = user.DateCreated
            };
        }

        public async Task<bool> RegisterUserAsync(UserRegister model)
        {
            if (await GetUserByEmailAsync(model.Email) != null || await GetUserByUserNameAsync(model.UserName) != null)
                return false;

            UserEntity entity = new()
            {
                Email = model.Email,
                UserName = model.UserName,
                Pasword = model.Password,
                DateCreated = DateTime.UtcNow
            };

            var passwordHasher = new PasswordHasher<UserEntity>();
            entity.Pasword = passwordHasher.HashPassword(entity, model.Password);

            await _context.Users.AddAsync(entity);
            int numberOfChanges = await _context.SaveChangesAsync();
            return numberOfChanges == 1;
        }

        private async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());
        }

        private async Task<UserEntity?> GetUserByUserNameAsync(string UserName)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.UserName.ToLower() == UserName.ToLower());
        }
    }
}