using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Proje7.Models;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proje7.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;
        private const string ExchangeCacheKey = "EXCHANGE_RATE_USD";
        private const string GasCacheKey = "GAS_KEY";

        public HomeController(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View(new MainViewModel
            {
                Oteller = new HotelSearchResponse
                {
                    Data = new HotelData
                    {
                        Hotels = new List<HotelItem>()
                    }
                },
                SehirAdi = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(SearchViewModel p)
        {
            MainViewModel viewModel = new MainViewModel();
            var girisGun = p.GirisGun;
            var cikisGun = p.CikisGun;
            var destId = await getDestId(p.Sehir);

            



        var rates = await getExchangeRate();

            decimal dolarKuru = rates.Rates["TRY"]; 
            decimal euroParitesi = rates.Rates["EUR"]; 
            decimal sterlinParitesi = rates.Rates["GBP"]; 
            decimal euroKuru = dolarKuru / euroParitesi;
            decimal sterlinKuru = dolarKuru / sterlinParitesi;

            // Gas Price api kotasý doldu
            var fuel = await getGasPrice();
            var turkeyGas = fuel.Result.FirstOrDefault(x => x.Country.Equals("Turkey", StringComparison.OrdinalIgnoreCase));
            viewModel.Benzin = turkeyGas.Gasoline;
            viewModel.Motorin = turkeyGas.Diesel;
            viewModel.LPG = turkeyGas.Lpg;

            //Idarelýk statýk veri
            //viewModel.Benzin = "54.64";
            //viewModel.Motorin = "56.24";
            //viewModel.LPG = "28.61";

            var weather = await getWeather(p.Sehir);

            var gemini = await GetGemini(p.Sehir);

            viewModel.Oteller = await getHotelsByDestId(destId, girisGun, cikisGun); // Oteller.Data içerisindeki HotelItem leri dolasacagýz
            viewModel.USD = Math.Round((double)dolarKuru, 4);
            viewModel.EUR = Math.Round((double)euroKuru, 4);
            viewModel.GBP = Math.Round((double)sterlinKuru, 4);

            var tr = new CultureInfo("tr-TR");

            viewModel.LPG =
            (
                Math.Round(
                    double.Parse(viewModel.LPG, tr) * viewModel.EUR,
                    2
                )
            ).ToString("F2", tr);

            viewModel.Motorin =
            (
                Math.Round(
                    double.Parse(viewModel.Motorin, tr) * viewModel.EUR,
                    2
                )
            ).ToString("F2", tr);

            viewModel.Benzin =
            (
                Math.Round(
                    double.Parse(viewModel.Benzin, tr) * viewModel.EUR,
                    2
                )
            ).ToString("F2", tr);


            //Havadurumu
            viewModel.SehirAdi = weather.Name;
            viewModel.Ýcon = "https://openweathermap.org/img/wn/"+weather.Weather[0].Icon+ ".png";
            viewModel.Derece = weather.Main.Temp.ToString("N2");
            viewModel.HavaDurumu = weather.Weather[0].Description;
            viewModel.Nem = weather.Main.Humidity;
            viewModel.Ruzgar = weather.Wind.Speed.ToString("N2");

            //Gemini Yemek
            viewModel.GununYemegiAdi = gemini.yemekBaslik;
            viewModel.GununYemegiTarifi = gemini.yemekAciklama;

            //Gemini Rota
            viewModel.Rota1Baslik = gemini.Rota1Baslik;
            viewModel.Rota1Detay = gemini.Rota1Aciklama;
            viewModel.Rota2Baslik = gemini.Rota2Baslik;
            viewModel.Rota2Detay = gemini.Rota2Aciklama;
            viewModel.Rota3Baslik = gemini.Rota3Baslik;
            viewModel.Rota3Detay = gemini.Rota3Aciklama;
            viewModel.Rota4Baslik = gemini.Rota4Baslik;
            viewModel.Rota4Detay = gemini.Rota4Aciklama;
            viewModel.Rota5Baslik = gemini.Rota5Baslik;
            viewModel.Rota5Detay = gemini.Rota5Aciklama;

            var debugGoruntusu = System.Text.Json.JsonSerializer.Serialize(viewModel, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true 
            });

            Console.WriteLine(debugGoruntusu);


            return View(viewModel);
        }

        public async Task<string> getDestId(string sehir)
        {
            var client = _httpClientFactory.CreateClient("Booking");
            var response = await client.GetFromJsonAsync<DestIdResult>("/api/v1/hotels/searchDestination?query=" + sehir);

            return response.Data[0].DestId;
        }
    
        public async Task<HotelSearchResponse?> getHotelsByDestId(string destId, DateOnly girisGun, DateOnly cikisGun)
        {
            var client = _httpClientFactory.CreateClient("Booking");
            var url = "https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id=" + destId + "&search_type=CITY&adults=1&children_age=0&room_qty=1&page_number=1&units=metric&temperature_unit=c&languagecode=tr&currency_code=TRY&arrival_date=" + girisGun.ToString("yyyy-MM-dd") + "&departure_date="+cikisGun.ToString("yyyy-MM-dd");
            var response = await client.GetFromJsonAsync<HotelSearchResponse>(url);
            if (response == null)
            {
                return null;
            }
            return response;
        }
    
        public async Task<ExchangeRateResponse?> getExchangeRate()
        {
            if (_memoryCache.TryGetValue(ExchangeCacheKey, out ExchangeRateResponse? cached))
            {
                return cached;
            }

            var client = _httpClientFactory.CreateClient("ExchangeRate");
            var response = await client.GetFromJsonAsync<ExchangeRateResponse>("/latest?base=USD");

            if(response == null)
            {
               return null;

            }
            _memoryCache.Set(
            ExchangeCacheKey,
            response,
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });
            return response;
        }
    
        public async Task<GasViewModel?> getGasPrice()
        {
            if (_memoryCache.TryGetValue(GasCacheKey, out GasViewModel? cachedGas))
            {
                return cachedGas;
            }

            var client = _httpClientFactory.CreateClient("GasPrice");
            var response = await client.GetFromJsonAsync<GasViewModel>("/europeanCountries");

            if(response == null)
            {
                           return null;

            }

            _memoryCache.Set(
            GasCacheKey,
            response,
            new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            }
        );

            return response;
        }
    
        public async Task<OpenWeatherResponse?> getWeather(string city)
        {
            var client = _httpClientFactory.CreateClient("OpenWeather");
            var response = await client.GetFromJsonAsync<OpenWeatherResponse?>("city?city=" + city + "&lang=tr");
            if(response != null)
            {
                return response;
            }
            return null;
    }

        public async Task<GeminiResponse?> GetGemini(string city)
        {
            //Gemýnýyý program.cs'de tanýmlayama gerek yok. 
            var client = _httpClientFactory.CreateClient();
            var apiKey = "<GEMÝNÝ APÝ KEY>"; 
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";


            var systemPrompt = $@"
            Sen bir seyahat ve gurme asistanýsýn. '{city}' þehri için þu formatta veri üret.
            Kurallar:
            1. Asla Markdown (```json) kullanma. Sadece saf JSON string döndür.
            2. Dili Türkçe olsun.
            3. Tam olarak 5 rota ve 1 yemek önerisi ver.
            4. JSON þemasý þu anahtarlara sahip olmalý (Senin C# classýnla birebir ayný):
            {{
                ""rota1Baslik"": ""..."", ""rota1Aciklama"": ""..."",
                ""rota2Baslik"": ""..."", ""rota2Aciklama"": ""..."",
                ""rota3Baslik"": ""..."", ""rota3Aciklama"": ""..."",
                ""rota4Baslik"": ""..."", ""rota4Aciklama"": ""..."",
                ""rota5Baslik"": ""..."", ""rota5Aciklama"": ""..."",
                ""yemekBaslik"": ""..."", ""yemekAciklama"": ""...""
            }}";

            // 3. Ýsteði Paketle
            var requestBody = new GeminiRequest
            {
                Contents = new List<Content>
        {
            new Content
            {
                Parts = new List<Part> { new Part { Text = systemPrompt } }
            }
        }
            };

            // 4. API'ye Gönder
            var httpResponse = await client.PostAsJsonAsync(url, requestBody);

            if (!httpResponse.IsSuccessStatusCode) throw new Exception("Gemini API Hatasý");

            var geminiRoot = await httpResponse.Content.ReadFromJsonAsync<GeminiApiRoot>();

            var jsonText = geminiRoot?.Candidates[0].Content.Parts[0].Text;

            if (string.IsNullOrEmpty(jsonText)) return null;

            jsonText = jsonText.Replace("```json", "").Replace("```", "").Trim();

            var finalResponse = JsonSerializer.Deserialize<GeminiResponse>(jsonText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });

            return finalResponse;
        }
    }
}
