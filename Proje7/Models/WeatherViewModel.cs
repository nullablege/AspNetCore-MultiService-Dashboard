using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Proje7.Models
{
    public class OpenWeatherResponse
    {
       
        [JsonPropertyName("weather")]
        public List<WeatherInfo> Weather { get; set; }

     
        [JsonPropertyName("main")]
        public MainInfo Main { get; set; }

        
        [JsonPropertyName("wind")]
        public WindInfo Wind { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    // 2. Alt Sınıflar
    public class WeatherInfo
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; } 
    }

    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public decimal Temp { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; } 
    }

    public class WindInfo
    {
        [JsonPropertyName("speed")]
        public decimal Speed { get; set; }
    }

}
