using Dal.Exeptions;
using Dal.Models;
using Dal.Models.HelperModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Repositories.Interfaces;

namespace Repositories.Repositories
{
    public class TokenExperementsRepository : ITokenExperementsRepository
    {
        private readonly ApplicationContext _db;

        public TokenExperementsRepository(ApplicationContext context)
        {
            _db = context;
        }

        public async Task<IQueryable<Experement>> GetAllByToken(int tokenId)
        {
            var token = await _db.Tokens
                .Include(x => x.Experements)
                .SingleOrDefaultAsync(u => u.id == tokenId);

            if (token == null)
                throw new NotFoundException();

            var experementsIds = token.Experements.Select(us => us.ExperementId).ToList();

            return _db.Experements.Where(s => experementsIds.Contains(s.id));
        }

        public async Task<Experement> GetByNameAndToken(string experementName, string token)
        {
            var experement = await _db.TokenExperements
                .Where(x => x.Experement.Name == experementName && x.TokenModel.Token == token)
                .Select(te => te.Experement)
                .FirstOrDefaultAsync();

            if(experement == null)
                throw new NotFoundException();

            return experement;
        }

        public async Task<bool> AddTokenExperement(int tokenId, int experementId)
        {
            var token = await _db.Tokens.FindAsync(tokenId);

            if(token == null)
                throw new NotFoundException();

            var experement = await _db.Experements.FindAsync(experementId);

            if(experement == null)
                throw new NotFoundException();

            var tokenExperement = new TokenExperement
            {
                TokenId = tokenId,
                ExperementId = experementId
            };

            token.Experements.Add(tokenExperement);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTokenExperement(int tokenId, int experementId)
        {
            var tokenExperement = await _db.TokenExperements.FirstOrDefaultAsync(x => x.TokenId == tokenId && x.ExperementId == experementId);

            if(tokenExperement == null)
                throw new NotFoundException();

            _db.TokenExperements.Remove(tokenExperement);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetRarestExperementId(string experementName)
        {
            // Ось цей запит можна було зробити напряму в бд,але я вирішив зробити в Ef: SELECT TOP 1 e.id AS Id, COUNT(te.ExperementId) AS CountOfUses
            //FROM Experements e
            //LEFT JOIN TokenExperements te ON e.id = te.ExperementId
            //WHERE e.Name = @experementName
            //GROUP BY e.id
            //ORDER BY CountOfUses; 

            var result = await _db.Experements
                .Where(x => x.Name == experementName)
                .GroupJoin(
                    _db.TokenExperements,
                    experiment => experiment.id,
                    tokenExperiment => tokenExperiment.ExperementId,
                    (experiment, tokenExperimentsGroup) => new
                    {
                        Id = experiment.id,
                        CountOfUses = tokenExperimentsGroup.Count()
                    })
                .DefaultIfEmpty()
                .OrderBy(x => x.CountOfUses)
                .Select(x => new
                {
                    Id = x.Id,
                    CountOfUses = x.CountOfUses
                })
                .FirstOrDefaultAsync();

            if(result == null)
                throw new NotFoundException();

            return result.Id;
        }

        public async Task<int> GetIdByPercentRatio(string experementName)
        {
            var param = new SqlParameter("@ExperementName", experementName);

            var result = _db.Database.SqlQuery<int>($"EXEC GetChosenExperementIdByExperementName @ExperementName = {experementName}").AsEnumerable().FirstOrDefault();

            if(result == 0)
            {
                var newResult = _db.Database.SqlQuery<int>($"EXEC GetExperementToAddToTokenExperement @ExperementName = {experementName}").AsEnumerable().FirstOrDefault();

                if(newResult == 0)
                    throw new NotFoundException();

                return newResult;
            }

            return result;
        }
    }
}
