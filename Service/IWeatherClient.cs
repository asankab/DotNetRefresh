public interface IWeatherClient
{
    Task<List<object>> GetDataAsync();
}