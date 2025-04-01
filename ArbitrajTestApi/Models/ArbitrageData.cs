using System.ComponentModel.DataAnnotations;

namespace ArbitrajTestApi.Models
{
    public class ArbitrageData
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string QuarterFutureSymbol { get; set; }
        public decimal QuarterFuturePrice { get; set; }
        public string BiQuarterFutureSymbol { get; set; }
        public decimal BiQuarterFuturePrice { get; set; }
        public decimal PriceDifference { get; set; }
        public decimal PercentageDifference { get; set; }
    }
}