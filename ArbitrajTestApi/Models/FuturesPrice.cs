using System.ComponentModel.DataAnnotations;

namespace ArbitrajTestApi.Models
{
    public class FuturesPrice
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}