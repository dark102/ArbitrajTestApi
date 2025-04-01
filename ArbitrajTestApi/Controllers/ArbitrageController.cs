using ArbitrajTestApi.Models;
using ArbitrajTestApi.Repositories;
using ArbitrajTestApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArbitrajTestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArbitrageController : ControllerBase
    {
        private readonly IArbitrageRepository _arbitrageRepository;
        private readonly ITrackedPairsRepository _trackedPairsRepository;
        private readonly IFuturesService _futuresService;
        private readonly ILogger<ArbitrageController> _logger;

        public ArbitrageController(
            IArbitrageRepository arbitrageRepository,
            ITrackedPairsRepository trackedPairsRepository,
            ILogger<ArbitrageController> logger,
            IFuturesService futuresService)
        {
            _arbitrageRepository = arbitrageRepository;
            _trackedPairsRepository = trackedPairsRepository;
            _logger = logger;
            _futuresService = futuresService;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<ArbitrageData>> GetLatestArbitrageData(
            [FromQuery] string quarterSymbol,
            [FromQuery] string biQuarterSymbol)
        {
            try
            {
                var data = await _arbitrageRepository.GetLatestArbitrageDataAsync(quarterSymbol, biQuarterSymbol);
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting latest arbitrage data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("historical")]
        public async Task<ActionResult<List<ArbitrageData>>> GetHistoricalArbitrageData(
            [FromQuery] string quarterSymbol,
            [FromQuery] string biQuarterSymbol,
            [FromQuery] DateTime? startTime = null,
            [FromQuery] DateTime? endTime = null
            )
        {
            try
            {
                startTime ??= DateTime.UtcNow.AddDays(-30);
                endTime ??= DateTime.UtcNow;

                var data = await _arbitrageRepository.GetHistoricalArbitrageDataAsync(quarterSymbol, biQuarterSymbol, startTime.Value, endTime.Value);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting historical arbitrage data");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("tracked-pairs")]
        public async Task<ActionResult<TrackedPairs>> AddTrackedPair([FromBody] TrackedPairs pair)
        {
            try
            {
                if (string.IsNullOrEmpty(pair.QuarterFutureSymbol) || string.IsNullOrEmpty(pair.BiQuarterFutureSymbol))
                {
                    return BadRequest("QuarterFutureSymbol and BiQuarterFutureSymbol are required");
                }
                if((await _futuresService.IsQuarterlyContractExistsAsync(pair.QuarterFutureSymbol))== false || (await _futuresService.IsQuarterlyContractExistsAsync(pair.BiQuarterFutureSymbol)) == false)
                {
                    return BadRequest("QuarterFutureSymbol orBi Quarter Future Symbol is not available on the exchange");
                }
                var result = await _trackedPairsRepository.AddAsync(pair);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding tracked pair");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}