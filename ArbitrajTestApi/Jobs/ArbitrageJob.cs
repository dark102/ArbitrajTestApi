using ArbitrajTestApi.Repositories;
using ArbitrajTestApi.Services;
using Hangfire;

namespace ArbitrajTestApi.Jobs
{
    public class ArbitrageJob
    {
        private readonly IArbitrageService _arbitrageService;
        private readonly ITrackedPairsRepository _trackedPairsRepository;
        private readonly IArbitrageRepository _arbitrageRepository;
        private readonly IFuturesService _futuresService;
        private readonly ILogger<ArbitrageJob> _logger;

        public ArbitrageJob(
            IArbitrageService arbitrageService,
            ITrackedPairsRepository trackedPairsRepository,
            IFuturesService futuresService,
            ILogger<ArbitrageJob> logger,
            IArbitrageRepository arbitrageRepository)
        {
            _arbitrageService = arbitrageService;
            _trackedPairsRepository = trackedPairsRepository;
            _futuresService = futuresService;
            _logger = logger;
            _arbitrageRepository = arbitrageRepository;
        }

        //TODO: ����� ����� �� ������ ���� ��������� ������� ��� � ���� � ��� ������� ����� ����������. ���� ������������� ��������� ��� ������������ �������� ������
        // ��� ������� ������������ ������� �� � ��������� ������ ������� ���� � LastDateOfEntry �  �� ������ ������ � ��������� � ���� ���������� ������� ����������.
        // ���� �� ����������� ������ 1500(������������ ���������������� ������������ ��������) �� ����� ������. ����� ������� ��������� ���������� �� � ������� ��������� �� ������������

        /// <summary>
        /// The task of calculating arbitration
        /// </summary>
        /// <param name="init">Software initialization or routine tasks</param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 3)]
        public async Task CalculateArbitrage(bool init = true)
        {
            try
            {
                _logger.LogInformation("Starting arbitrage calculation");

                var trackedPairs = await _trackedPairsRepository.GetAllAsync();
                if (trackedPairs == null || !trackedPairs.Any())
                {
                    _logger.LogInformation("No tracked pairs found in database");
                    return;
                }

                foreach (var pair in trackedPairs)
                {
                    if (pair.LastDateOfEntry == default)
                    {
                        pair.LastDateOfEntry = DateTime.UtcNow.AddMinutes(-100);
                        await _trackedPairsRepository.UpdateLastDateOfEntryAsync(pair.Id, pair.LastDateOfEntry);
                    }
                    DateTime? start = null;
                    DateTime? end = init?  DateTime.UtcNow.AddMinutes(-2) : DateTime.UtcNow.AddMinutes(-1);
                    int limit = 100;
                    if(init)
                    {
                        start = pair.LastDateOfEntry;
                        
                        limit = 1500;
                    }

                    await _arbitrageService.CalculateAndSaveHistoryArbitrageAsync(
                        pair.QuarterFutureSymbol,
                        pair.BiQuarterFutureSymbol,
                        start,
                        end,
                        limit
                    );

                    var lastDate = await _arbitrageRepository.GetLatestArbitrageDataAsync(pair.QuarterFutureSymbol, pair.BiQuarterFutureSymbol);
                    await _trackedPairsRepository.UpdateLastDateOfEntryAsync(pair.Id, lastDate.Timestamp);
                }

                _logger.LogInformation("Arbitrage calculation completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during arbitrage calculation");
                throw;
            }
        }
    }
}