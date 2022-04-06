using System.Threading.Tasks;
using Exercise.Models;

namespace Exercise.Repositories.Interfaces
{
    public interface IUserDbRepository
    {
        Task<int> VerifyUserNameFromDbAsync(string username);
    }
}