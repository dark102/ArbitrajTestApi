namespace ArbitrajTestApi.Models
{
    public class TrackedPairs
    {
        public int Id { get; set; }
        public string QuarterFutureSymbol { get; set; }
        public string BiQuarterFutureSymbol { get; set; }
        public bool isNew { get; set; } = true;
        public DateTime LastDateOfEntry { get; set; }
    }
}
