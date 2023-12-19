namespace TaskTrackerApi.Data
{
    public interface IDbInitializer
    {
        void Initialize(TaskApiContext context);
    }
}
