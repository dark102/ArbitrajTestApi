using ArbitrajTestApi.Models;
using ArbitrajTestApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArbitrajTestApi.Services
{
    public class ArbitrageService : IArbitrageService
    {
        private readonly IFuturesService _futuresService;
        private readonly IArbitrageRepository _repository;
        private readonly ILogger<ArbitrageService> _logger;

        public ArbitrageService(
            IFuturesService futuresService,
            IArbitrageRepository repository,
            ILogger<ArbitrageService> logger)
        {
            _futuresService = futuresService;
            _repository = repository;
            _logger = logger;
        }
        public async Task CalculateAndSaveHistoryArbitrageAsync(
            string quarterFutureSymbol,
            string biQuarterFutureSymbol,
            DateTime? startTime,
            DateTime? endTime,
            int limit)
        {
            try
            {
                var quarterFuture = (await _futuresService.GetHistoricalPricesAsync(quarterFutureSymbol, startTime, endTime, limit)).OrderByDescending(a => a.Timestamp).ToList();
                var biQuarterFuture = (await _futuresService.GetHistoricalPricesAsync(biQuarterFutureSymbol, startTime, endTime, limit)).OrderByDescending(a => a.Timestamp).ToList();

                if (quarterFuture == null || biQuarterFuture == null)
                {
                    _logger.LogWarning("No data found for the specified time range");
                    return;
                }
                
                foreach (var quarterItem in quarterFuture)
                {
                    var biquarterItem = biQuarterFuture.FirstOrDefault(x => x.Timestamp == quarterItem.Timestamp);
                    
                    var prices = await CheckPrice(quarterFutureSymbol, quarterItem?.Price, biQuarterFutureSymbol, biquarterItem?.Price);

                    var priceDifference = prices.quarterPrice - prices.biQuarterPrice;

                    var percentageDifference = (priceDifference / prices.quarterPrice) * 100;
                    var arbitrageData = new ArbitrageData
                    {
                        Timestamp = quarterItem.Timestamp,
                        QuarterFutureSymbol = quarterFutureSymbol,
                        QuarterFuturePrice = prices.quarterPrice,
                        BiQuarterFuturePrice = prices.biQuarterPrice,
                        BiQuarterFutureSymbol = biQuarterFutureSymbol,
                        PriceDifference = priceDifference,
                        PercentageDifference = percentageDifference
                    };

                    await _repository.SaveArbitrageDataAsync(arbitrageData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating arbitrage data");
                throw;
            }
        }

        /// <summary>
        /// Checking the contract price. if the price is zero, then the last one from the database is taken.
        /// </summary>
        /// <param name="quarterSymbol"></param>
        /// <param name="quarterPrice"></param>
        /// <param name="biQuarterSymbol"></param>
        /// <param name="biQuarterPrice"></param>
        /// <returns></returns>
        private async Task<(decimal quarterPrice, decimal biQuarterPrice)> CheckPrice(
            string quarterSymbol, 
            decimal? quarterPrice,
            string biQuarterSymbol, 
            decimal? biQuarterPrice)
        {
            if (quarterPrice == null || quarterPrice == 0 || biQuarterPrice == null || biQuarterPrice == 0)
            {
                var last = await _repository.GetLatestArbitrageDataAsync(quarterSymbol, biQuarterSymbol);
                if(quarterPrice == null || quarterPrice == 0)
                    quarterPrice = last.QuarterFuturePrice;
                if (biQuarterPrice == null || biQuarterPrice == 0)
                    biQuarterPrice = last.BiQuarterFuturePrice;
            }
            return ((decimal)quarterPrice, (decimal)biQuarterPrice);
        }
    }
}