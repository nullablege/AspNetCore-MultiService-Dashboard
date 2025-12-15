using System.Text.Json.Serialization;

namespace Proje7.Models
{
    // 1. Root
    public class HotelSearchResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public HotelData Data { get; set; }
    }

    // 2. Data
    public class HotelData
    {
        [JsonPropertyName("hotels")]
        public List<HotelItem> Hotels { get; set; }
    }

    // 3. Hotel Item (Listeden gelen her bir otel)
    public class HotelItem
    {
        [JsonPropertyName("accessibilityLabel")]
        public string AccessibilityLabel { get; set; }

        [JsonPropertyName("property")]
        public HotelProperty Property { get; set; }
    }

    // 4. Property Detayları
    public class HotelProperty
    {
        [JsonPropertyName("reviewScore")]
        public double ReviewScore { get; set; } 

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("photoUrls")]
        public List<string> PhotoUrls { get; set; }


    }
}