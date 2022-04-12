using System.Threading.Tasks;
using Exercise.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exercise.Repositories.Interfaces
{
    public interface IUserDbRepository
    {
        public Task<List<User>> GetUsers();

        public Task<bool> DeleteUser(string username);

        public Task<bool> AddUser(string username);

        public Task<bool> ModifyUser(string username, string newusername);

        public Task<bool> VerifyUsername(string username);
    }
}