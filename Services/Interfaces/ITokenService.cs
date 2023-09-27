namespace Services.Interfaces
{
    public interface ITokenService
    {
        Task<bool> TryToCreateNewToken(string token);
    }
}
