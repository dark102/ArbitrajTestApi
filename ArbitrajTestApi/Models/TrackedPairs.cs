namespace ArbitrajTestApi.Models
{
    public class TrackedPairs
    {
        public int Id { get; set; }
        public string QuarterFutureSymbol { get; set; }
        public string BiQuarterFutureSymbol { get; set; }
        public DateTime LastDateOfEntry { get; set; }
        public DateTime EndDateOfTracking { get; set; }
    }
}
