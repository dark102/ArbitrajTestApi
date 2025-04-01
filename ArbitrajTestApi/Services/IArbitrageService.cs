using ArbitrajTestApi.Models;

namespace ArbitrajTestApi.Services
{
    public interface IArbitrageService
    {
        /// <summary>
        /// Calculation of the arbitrage for the specified period, indicating the number of candles in the response
        /// </summary>
        /// <param name="quarterFutureSymbol">First pair</param>
        /// <param name="biQuarterFutureSymbol">Second pair</param>
        /// <param name="startTime">Beginning of the period</param>
        /// <param name="endTime">End of the period</param>
        /// <param name="limit">Number of candles</param>
        /// <returns></returns>
        Task CalculateAndSaveHistoryArbitrageAsync(string quarterFutureSymbol, string biQuarterFutureSymbol, DateTime? startTime, DateTime? endTime, int limit); 
    }
}