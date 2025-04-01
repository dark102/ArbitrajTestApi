using ArbitrajTestApi.Models;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Options;
using CryptoExchange.Net.Authentication;

namespace ArbitrajTestApi.Services
{
    /// <summary>
    /// Futures service
    /// </summary>
    public class FuturesService : IFuturesService
    {
        private readonly ILogger<FuturesService> _logger;
        private readonly BinanceRestClient _binanceClient;

        public FuturesService(ILogger<FuturesService> logger)
        {
            _logger = logger;
            _binanceClient = new BinanceRestClient();
        }
        
        public async Task<bool> IsQuarterlyContractExistsAsync(string symbol)
        {
            var exchangeInfo = await _binanceClient.UsdFuturesApi.ExchangeData.GetExchangeInfoAsync();
            return exchangeInfo.Data.Symbols.Any(s => s.Name == symbol);
        }

        public async Task<List<FuturesPrice>> GetHistoricalPricesAsync(string symbol, DateTime? startTime, DateTime? endTime, int limit)
        {
            try
            {
                var result = await _binanceClient.UsdFuturesApi.ExchangeData.GetKlinesAsync(
                    symbol,
                    KlineInterval.OneMinute,
                    startTime,
                    endTime,
                    limit
                );

                if (result.Success)
                {
                    return result.Data.Select(kline => new FuturesPrice
                    {
                        Symbol = symbol,
                        Price = kline.ClosePrice,
                        Timestamp = kline.CloseTime
                    }).ToList();
                }
                throw new Exception($"Failed to get historical prices for {symbol}: {result.Error}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting historical prices for {Symbol}", symbol);
                throw;
            }
        }
    }
}