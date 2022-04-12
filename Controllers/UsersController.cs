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
        public async Task<IActionResult> GetUsers()
        {
            var getUser = await _repository.GetUsers();
            return Ok(getUser);
            }


        [HttpGet("{username}")]
        public async Task<IActionResult> VerifyUsername(string username)
        {
            var verification = await _repository.VerifyUsername(username);
            if(!verification)
            {
                return BadRequest("Username doesn't meet requirements or is already taken!");
            }

            return Ok("Username meets requirements!");

        }


        [HttpPut("{username}")]
        public async Task<IActionResult> ModifyUser(string username, string newusername)
        {
           
            var feedback = await _repository.ModifyUser(username, newusername);
            if(!feedback)
            {
                return BadRequest("Username doesn't meet requirements!"); 
            }
            return Ok("Username updated to " + newusername+"!");
           
        }

        
        [HttpPost("{username}")]
        public async Task<IActionResult> AddUser(string username)
        {
            var addUser = await _repository.AddUser(username);
            if (!addUser)
            {
                return BadRequest("Username doesn't meet requirements or is already taken!");
            }
            return Ok("User " + username + " added!");


        }

        
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var deleteUser = await _repository.DeleteUser(username);
            if (!deleteUser)
            {
                return BadRequest("User doesn't exist!");
            }
            return Ok("User deleted!");
        }

        


    }


}

    

