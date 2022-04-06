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

        public UsersController(ExerciseContext context,IConfiguration configuration,IUserDbRepository repository)
        {
            _context = context;
            _configuration = configuration;
            _repository = repository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("validateusername")]
        public async Task<ActionResult> VerifyUsername(string username)
        {
            var result = await _repository.VerifyUserNameFromDbAsync(username);
            if (result == 1)
            {
                return BadRequest("User with this login already exists");
            }
            
            if (username == null)
            {
                return BadRequest("No username specified!");
            }

            if (username.Length>6 && username.Length<30 && Regex.IsMatch(username, "^[a-zA-Z0-9]*$"))
            {
                return Ok("Username meets requirements");
            }

            return BadRequest("Username doesn't meet requirements");
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> ModifyUser(Guid id, string username)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found!");
            }

            var result = await _repository.VerifyUserNameFromDbAsync(username);
            if (result == 1)
            {
                return BadRequest("User with this login already exists");
            }

            if (username == null)
            {
                return BadRequest("No username specified!");
            }

            if (username.Length > 6 && username.Length < 30 && Regex.IsMatch(username, "^[a-zA-Z0-9]*$"))
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

            

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("adduser")]
        public async Task<ActionResult<User>> AddUser(string username)
        {

            //verifying the username TODO create a function for that

            var result = await _repository.VerifyUserNameFromDbAsync(username);
            if (result == 1)
            {
                return BadRequest("User with this login already exists");
            }

            if (username == null)
            {
                return BadRequest("No username specified!");
            }

            if (username.Length > 6 && username.Length < 30 && Regex.IsMatch(username, "^[a-zA-Z0-9]*$"))
            {

                //adding verified username to the database
                User newuser = new User();
                newuser.UserName = username;  
                _context.Users.Add(newuser);
                await _context.SaveChangesAsync();

                return Ok(username + " Added!");

            }

            return BadRequest("Username doesn't meet requirements");

            
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
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
    }
}
