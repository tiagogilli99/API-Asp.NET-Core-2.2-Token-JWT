using System.Threading.Tasks;
using API.Data.Context;
using API.Data.Repository;
using API.Domain.Entities;
using API.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Implementations
{
    public class UserImplementation : BaseRepository<UserEntity>, IUserRepository
    {
        private DbSet<UserEntity> _dataSet;

        public UserImplementation(MyContext context) : base(context)
        {
            _dataSet = context.Set<UserEntity>();
        }

        public async Task<UserEntity> FindByLogin(string email)
        {
            return await _dataSet.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
    }
}
