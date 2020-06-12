using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp2.Models;

namespace DatingApp2.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T: class;
        void Delete<T>(T entity) where T:class;
        Task<bool> SaveAll();
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
    }
}