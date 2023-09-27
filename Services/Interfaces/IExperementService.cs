using Dal.Models.DtoModels;

namespace Services.Interfaces
{
    public interface IExperementService
    {
        Task AddExperements(int tokenId);
        Task<ViewExperement> GetExperementByTokenAndName(string experementName, string token);
    }
}
