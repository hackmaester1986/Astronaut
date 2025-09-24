using StargateApi.Repositories;

namespace StargateAPI.Services.Impl
{
    public class AstronautDetailService : IAstronautDetailService
    {
        private readonly IAstronautDetailRepository _astronautDetailRepository;

        public AstronautDetailService(IAstronautDetailRepository astronautDetailRepository)
        {
            _astronautDetailRepository = astronautDetailRepository;
        }

        public async Task<bool> CreateAstronautDetail(string name, string rank, string title)
        {
            return await _astronautDetailRepository.CreateAstronautDetail(name, rank, title);
        }
    }
}