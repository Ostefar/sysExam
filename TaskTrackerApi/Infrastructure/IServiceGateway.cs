namespace TaskTrackerApi.Infrastructure
{
    public interface IServiceGateway<T>
    {
        Task<T> GetAsync(int id);
    }
}