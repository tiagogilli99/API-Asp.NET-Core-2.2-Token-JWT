using System.Threading.Tasks;
using API.Domain.Entities;
using API.Domain.Interfaces;

namespace API.Domain.Repository
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity> FindByLogin(string email);
    }
}
