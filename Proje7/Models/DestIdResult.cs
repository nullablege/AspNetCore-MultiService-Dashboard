using System.Text.Json.Serialization;

namespace Proje7.Models
{
    public class DestIdResult
    {
        [JsonPropertyName("data")]
        public List<SearchItem> Data { get; set; }
    }
    public class SearchItem
    {
        [JsonPropertyName("dest_id")]
        public string? DestId { get; set; }
    }
}
