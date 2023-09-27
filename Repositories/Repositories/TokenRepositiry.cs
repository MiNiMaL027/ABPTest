using Dal.Exeptions;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class TokenRepositiry : ITokenRepository
    {
        private readonly ApplicationContext _db;

        public TokenRepositiry(ApplicationContext context)
        {
            _db = context;
        }

        public async Task<bool> IsTokenExist(string token)
        {
            return await _db.Tokens.AnyAsync(x => x.Token == token);
        }

        public async Task<bool> AddToken(string token)
        {
            var newToken = new TokenModel { Token = token };

            await _db.Tokens.AddAsync(newToken);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetTokenIdByToken(string token)
        {
            var DbToken = await _db.Tokens.FirstOrDefaultAsync(x => x.Token == token);

            if (DbToken == null)
                throw new NotFoundException();

            return DbToken.id;
        }
    }
}
