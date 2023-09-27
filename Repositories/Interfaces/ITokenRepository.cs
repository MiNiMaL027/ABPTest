namespace Repositories.Interfaces
{
    public interface ITokenRepository
    {
        Task<bool> IsTokenExist(string token);
        Task<bool> AddToken(string token);
        Task<int> GetTokenIdByToken(string token);
    }
}
