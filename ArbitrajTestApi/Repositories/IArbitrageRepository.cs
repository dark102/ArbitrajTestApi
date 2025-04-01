using ArbitrajTestApi.Models;

namespace ArbitrajTestApi.Repositories
{
    public interface IArbitrageRepository
    {
        Task SaveArbitrageDataAsync(ArbitrageData arbitrageData);
        Task<List<ArbitrageData>> GetHistoricalArbitrageDataAsync(string quarterSymbol, string biQuarterSymbol, DateTime startTime, DateTime endTime);
        Task<ArbitrageData> GetLatestArbitrageDataAsync(string quarterSymbol, string biQuarterSymbol);
    }
}