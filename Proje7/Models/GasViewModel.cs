using Newtonsoft.Json;

namespace Proje7.Models
{
    public class GasViewModel
    {
        [JsonProperty("result")]
        public List<Result> Result { get; set; }
    }

    public class Result
    {
        public string Currency {  get; set; }
        public string Lpg { get; set; }
        public string Diesel { get; set; }
        public string Gasoline { get; set; }
        public string Country { get; set; }
    }
}
