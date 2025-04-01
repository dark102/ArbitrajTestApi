using ArbitrajTestApi.Data;
using ArbitrajTestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArbitrajTestApi.Repositories
{
    public class ArbitrageRepository : IArbitrageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ArbitrageRepository> _logger;

        public ArbitrageRepository(ApplicationDbContext context, ILogger<ArbitrageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SaveArbitrageDataAsync(ArbitrageData arbitrageData)
        {
            try
            {
                await _context.ArbitrageData.AddAsync(arbitrageData);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving arbitrage data");
                throw;
            }
        }

        public async Task<List<ArbitrageData>> GetHistoricalArbitrageDataAsync(string quarterSymbol, string biQuarterSymbol, DateTime startTime, DateTime endTime)
        {
            try
            {
                return  (await _context.ArbitrageData.ToListAsync())
                    .Where(a => 
                    a.Timestamp >= startTime && 
                    a.Timestamp <= endTime && a.QuarterFutureSymbol == quarterSymbol && a.BiQuarterFutureSymbol == biQuarterSymbol)
                    .OrderByDescending(a => a.Timestamp)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting historical arbitrage data");
                throw;
            }
        }

        public async Task<ArbitrageData> GetLatestArbitrageDataAsync(string quarterSymbol, string biQuarterSymbol)
        {
            try
            {
                return await _context.ArbitrageData
                    .Where(x=> x.QuarterFutureSymbol == quarterSymbol && x.BiQuarterFutureSymbol == biQuarterSymbol)
                    .OrderByDescending(x => x.Timestamp)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest arbitrage data");
                throw;
            }
        }
    }
}