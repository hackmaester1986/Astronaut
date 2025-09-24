namespace StargateApi.Repositories
{
    public interface IAstronautDetailRepository
    {
        Task<bool> CreateAstronautDetail(string name,string rank, string title);

    }
}