#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exercise.Models;
using System.Text.RegularExpressions;
using Exercise.Repositories.Interfaces;

namespace Exercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ExerciseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserDbRepository _repository;

        public UsersController(ExerciseContext context, IConfiguration configuration, IUserDbRepository repository)
        {
            _context = context;
            _configuration = configuration;
            _repository = repository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }


        [HttpGet("{username}")]
        public ActionResult VerifyUsername(string username)
        {

            if (UsernameExists(username))
            {
                return BadRequest("User with this login already exists");
            }

            if (!UsernameInputted(username))
            {
                return BadRequest("No username specified!");
            }
            if (!UsernamePassRegex(username))
            {
                return BadRequest("Username doesn't meet requirements");
            }

            return Ok("Username meets requirements");

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyUser(Guid id, string username)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (!UserExists(id))
            {
                return NotFound("User not found!");
            }

            if (!UsernameInputted(username))
            {
                return BadRequest("No username specified!");
            }

            if (UsernamePassRegex(username))
            {
                user.UserName = username;
                
                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok("Username changed to " + username);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }

            return BadRequest("Username doesn't meet requirements");
           
        }

        
        [HttpPost("{username}")]
        public async Task<ActionResult<User>> AddUser(string username)
        {
            if (UsernameExists(username))
            {
                return NotFound("User with this login already exists!");
            }

            if (!UsernameInputted(username))
            {
                return BadRequest("No username specified!");
            }

            if (UsernamePassRegex(username))
            {
                User newuser = new User();
                newuser.UserName = username;
                _context.Users.Add(newuser);
                await _context.SaveChangesAsync();

                return Ok(username + " Added!");
            }

            return BadRequest("Username doesn't meet requirements");

            
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (!UserExists(id))
            {
                return NotFound("User not found!");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool UsernameExists(string username)
        {
            return _context.Users.Any(e => e.UserName == username);
        }

        private bool UsernameInputted(string username)
        {
            if (username == null)
            {
                return false;
            }
            return true;
        }

        private bool UsernamePassRegex(string username)
        {
            if (username.Length > 6 && username.Length < 30 && Regex.IsMatch(username, "^[a-zA-Z0-9]*$"))
            {
                return true;
            }

            return false;
        }


    }


}

    

