using ArbitrajTestApi.Models;

namespace ArbitrajTestApi.Services
{
    public interface IFuturesService
    {
        /// <summary>
        /// Checks if a quarterly contract exists
        /// </summary>
        /// <param name="symbol">Pair</param>
        /// <returns></returns>
        Task<bool> IsQuarterlyContractExistsAsync(string symbol);

        /// <summary>
        /// Returns the historical prices of a futures contract
        /// </summary>
        /// <param name="symbol">Pair</param>
        /// <param name="startTime">Beginning of the period</param>
        /// <param name="endTime">End of the period</param>
        /// <param name="limit">Number of candles</param>
        /// <returns></returns>
        Task<List<FuturesPrice>> GetHistoricalPricesAsync(string symbol, DateTime? startTime, DateTime? endTime, int limit);
    }
}