namespace StargateAPI.Services
{
    public interface IAstronautDetailService
    {
        Task<bool> CreateAstronautDetail(string name, string rank, string title,DateTime? careerStartDate);
        Task<bool> UpdateAstronautDetailAsync(AstronautDetailRequestDto updatedDetail);
        Task<AstronautDetailDto> GetDetailByName(string name);
    }
}