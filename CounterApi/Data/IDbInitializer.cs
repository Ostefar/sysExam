namespace CounterApi.Data
{
    public interface IDbInitializer
    {
        void Initialize(CounterApiContext context);
    }
}
