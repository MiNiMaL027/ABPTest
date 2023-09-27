using AutoMapper;
using Dal.Exeptions;
using Dal.Models.DtoModels;
using Newtonsoft.Json.Linq;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class ExperementService : IExperementService
    {
        private readonly ITokenExperementsRepository _tokenExperementsRepository;
        private readonly IMapper _mapper;

        public ExperementService(ITokenExperementsRepository tokenExperementsRepository, IMapper mapper)
        {
            _tokenExperementsRepository = tokenExperementsRepository;
            _mapper = mapper;
        }

        public async Task AddExperements(int tokenId)
        {
            var expId = await _tokenExperementsRepository.GetRarestExperementId("button_color");

            await _tokenExperementsRepository.AddTokenExperement(tokenId, expId);

            await _tokenExperementsRepository.AddTokenExperement(
                tokenId,
                await _tokenExperementsRepository.GetIdByPercentRatio("price")
            );
        }

        public async Task<ViewExperement> GetExperementByTokenAndName(string experementName, string token)
        {
            if (experementName == null)
                throw new ValidationException();

            if(token == null)
                throw new ValidationException();

            var dbExperement = await _tokenExperementsRepository.GetByNameAndToken(experementName, token);

            return _mapper.Map<ViewExperement>(dbExperement);
        }
    }
}
