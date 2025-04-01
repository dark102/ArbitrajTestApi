using System.ComponentModel.DataAnnotations;

namespace ArbitrajTestApi.Models
{
    public class Log
    {
        [Key]
        public long id { get; set; }

        public string message { get; set; } = string.Empty;
        public string messagetemplate { get; set; } = string.Empty;
        public string message_template { get; set; } = string.Empty;
        public string level { get; set; } = string.Empty;
        public DateTime timestamp { get; set; }
        public string exception { get; set; } = string.Empty;
        public string properties { get; set; } = string.Empty;
        public string logevent { get; set; } = string.Empty;
        public string log_event { get; set; } = string.Empty;
        
    }
}