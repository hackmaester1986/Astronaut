namespace StargateAPI.Services
{
    public interface IAstronautDetailService
    {
        Task<bool> CreateAstronautDetail(string name, string rank, string title);
    }
}