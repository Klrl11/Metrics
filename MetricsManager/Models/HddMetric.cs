using Newtonsoft.Json;

namespace MetricsManager.Models
{
    public class HddMetric
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}
