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

        public async Task<bool> CreateAstronautDetail(string name, string rank, string title,DateTime? careerStartDate)
        {
            return await _astronautDetailRepository.CreateAstronautDetail(name, rank, title,careerStartDate);
        }

        public async Task<bool> UpdateAstronautDetailAsync(AstronautDetailRequestDto updatedDetail)
        {
            return await _astronautDetailRepository.UpdateAstronautDetailAsync(updatedDetail);
        }

        public async Task<AstronautDetailDto> GetDetailByName(string name)
        {
            return await _astronautDetailRepository.GetDetailByName(name);
        }
    }
}