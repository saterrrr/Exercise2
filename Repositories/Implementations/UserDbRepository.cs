using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Exercise.Models;
using Exercise.Repositories.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;




namespace Exercise.Repositories.Implementations
{
    public class UserDbRepository : IUserDbRepository
    {
        private readonly ExerciseContext _context;

        public UserDbRepository(ExerciseContext context)
        {
            _context = context;
        }

        
        async Task<List<User>> IUserDbRepository.GetUsers()
        {
            return await _context.Users.ToListAsync();   
           

        }
        

        async Task<bool> IUserDbRepository.DeleteUser(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
            if (!UsernameExists(username))
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        async Task<bool> IUserDbRepository.AddUser(string username)
        {   
            if (username == null || UsernameExists(username) || !UsernamePassRegex(username))
            {
                return false;
            }

            await _context.AddAsync(new User
            {
                UserName = username,

            });


            await _context.SaveChangesAsync();

            return true;
        }

        async Task<bool> IUserDbRepository.ModifyUser(string username, string newusername)
        {
            var userr = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
            if (userr != null && UsernamePassRegex(newusername)==true)
            {
                userr.UserName = newusername;
                await _context.SaveChangesAsync();
                return true;

            }

            return false;
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool UsernameExists(string username)
        {
            return _context.Users.Any(e => e.UserName == username);
        }

        private bool UsernamePassRegex(string username)
        {
            if (username.Length > 6 && username.Length < 30 && Regex.IsMatch(username, "^[a-zA-Z0-9]*$"))
            {
                return true;
            }

            return false;
        }

        public async Task<bool> VerifyUsername (string username)
        {
            if (UsernameExists(username))
            {
                return false;
            }

            if (username==null)
            {
                return false;
            }
            if (!UsernamePassRegex(username))
            {
                return false;
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}