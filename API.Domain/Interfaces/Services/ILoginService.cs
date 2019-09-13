using System.Threading.Tasks;
using API.Domain.DTO;

namespace API.Domain.Interfaces.Services
{
    public interface ILoginService
    {
        Task<object> FindByLogin(LoginDTO user);
    }
}
