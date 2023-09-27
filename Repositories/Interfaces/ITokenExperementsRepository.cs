using Dal.Models;

namespace Repositories.Interfaces
{
    public interface ITokenExperementsRepository
    {
        Task<IQueryable<Experement>> GetAllByToken(int tokenId);
        Task<bool> AddTokenExperement(int tokenId, int experementId);
        Task<bool> DeleteTokenExperement(int tokenId, int experementId);
        Task<int> GetRarestExperementId(string experementName);
        Task<Experement> GetByNameAndToken(string experementName, string token);
        Task<int> GetIdByPercentRatio(string experementName);
    }
}
