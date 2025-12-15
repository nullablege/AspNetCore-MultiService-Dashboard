using System.Text.Json.Serialization;

namespace Proje7.Models
{

    public class ExchangeRateResponse
    {
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }

}
