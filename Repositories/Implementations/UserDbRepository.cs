using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Exercise.Models;
using Exercise.Repositories.Interfaces;

namespace Exercise.Repositories.Implementations
{
    public class UserDbRepository : IUserDbRepository
    {
        private readonly ExerciseContext _context;

        public UserDbRepository(ExerciseContext context)
        {
            _context = context;
        }

        public async Task<int> VerifyUserNameFromDbAsync(string username)
        {
            User userentered = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
            if (userentered != null)
            {
                return 1;
            }


           
            await _context.SaveChangesAsync();
            return 0;
        }

    }
}