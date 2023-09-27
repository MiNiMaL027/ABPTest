using Dal.Exeptions;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IExperementService _experementService;

        public TokenService(ITokenRepository tokenRepository, IExperementService experementService)
        {
            _tokenRepository = tokenRepository;
            _experementService = experementService;
        }

        public async Task<bool> TryToCreateNewToken(string token)
        {
            if (token == null)
                throw new ValidationException();

            if(await _tokenRepository.IsTokenExist(token))
                return false;

            await _tokenRepository.AddToken(token);

            var newTokenId = await _tokenRepository.GetTokenIdByToken(token);

            await _experementService.AddExperements(newTokenId);   
            
            return true;
        }
    }
}
