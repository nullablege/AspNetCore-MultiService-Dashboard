# 🌐 AspNetCore MultiService Dashboard

![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white)
![RapidAPI](https://img.shields.io/badge/RapidAPI-Integrated-0055FF?style=for-the-badge&logo=api&logoColor=white)
![Google Gemini](https://img.shields.io/badge/Google%20Gemini-AI-4285F4?style=for-the-badge&logo=google&logoColor=white)

Birden fazla harici API'yi entegre eden, modern ve kapsamlı bir **ASP.NET Core 8.0 MVC Dashboard** uygulaması. Bu proje, **M&Y Akademi** Full Stack .Net bottcamp kapsamında geliştirilmiştir.

---

## 📖 Proje Hakkında

Bu proje, farklı dış servislerden veri çekerek kullanıcıya tek bir dashboard üzerinde zengin içerik sunan bir web uygulamasıdır. Gerçek zamanlı hava durumu, döviz kurları, akaryakıt fiyatları, otel aramaları ve **Google Gemini AI** destekli yapay zeka özellikleri içermektedir.

### 🎯 Projenin Amacı

- Modern **API tüketim (consume)** tekniklerini uygulamak
- **HttpClient Factory Pattern** ile servis yönetimi
- **In-Memory Caching** ile performans optimizasyonu
- Birden fazla veri kaynağını tek bir ViewModel'de birleştirmek
- Responsive ve modern bir kullanıcı arayüzü sunmak

---

## ✨ Özellikler

### 🔌 Entegre Edilen API'ler

| API | Açıklama | Veri Türü |
|-----|----------|-----------|
| **OpenWeather API** | Anlık hava durumu verileri | Sıcaklık, Nem, Rüzgar, Hava Durumu |
| **Exchange Rates API** | Döviz kuru bilgileri | USD/TRY, EUR/TRY, GBP/TRY |
| **Gas Price API** | Akaryakıt fiyatları | Benzin, Motorin, LPG |
| **Booking.com API** | Otel arama ve listeleme | Otel bilgileri, Fotoğraflar, Fiyatlar |
| **Google Gemini AI** | Yapay zeka destekli içerik | Yemek önerileri, Gezi rotaları |

### 🚀 Teknik Özellikler

- **HttpClient Factory**: Named HttpClient'lar ile API yönetimi
- **In-Memory Cache**: Gereksiz API çağrılarını önleme
- **Dependency Injection**: Modern servis mimarisi
- **Async/Await Pattern**: Non-blocking API çağrıları
- **JSON Serialization**: System.Text.Json ile veri dönüşümü
- **Responsive Design**: Bootstrap tabanlı modern arayüz

---

## 🏗️ Proje Mimarisi

```
Proje7/
├── Controllers/
│   └── HomeController.cs       # Ana controller - tüm API çağrıları
├── Models/
│   ├── MainViewModel.cs        # Ana ViewModel - tüm verileri birleştirir
│   ├── WeatherViewModel.cs     # Hava durumu veri modeli
│   ├── ExchangeRateResponse.cs # Döviz kuru yanıt modeli
│   ├── GasViewModel.cs         # Akaryakıt fiyat modeli
│   ├── SearchHotelResponse.cs  # Otel arama yanıt modeli
│   ├── GeminiRequest.cs        # Gemini AI istek modeli
│   └── GeminiResponse.cs       # Gemini AI yanıt modeli
├── Views/
│   ├── Home/
│   │   └── Index.cshtml        # Ana dashboard görünümü
│   └── Shared/
│       └── _Layout.cshtml      # Ana layout
├── wwwroot/
│   ├── css/
│   │   └── styles.css          # Custom stil dosyası
│   ├── js/
│   │   └── main.js             # JavaScript fonksiyonları
│   └── lib/                    # Bootstrap, jQuery vb.
└── Program.cs                  # Uygulama konfigürasyonu
```

---

## 🔧 HttpClient Factory Yapılandırması

Proje, Named HttpClient pattern'i kullanarak farklı API'ler için özelleştirilmiş HTTP istemcileri tanımlar:

```csharp
// Program.cs
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

// Booking.com API Client
builder.Services.AddHttpClient("Booking", client =>
{
    client.BaseAddress = new Uri("https://booking-com15.p.rapidapi.com");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "<RAPID_API_KEY>");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "booking-com15.p.rapidapi.com");
});

// Exchange Rate API Client
builder.Services.AddHttpClient("ExchangeRate", client =>
{
    client.BaseAddress = new Uri("https://exchange-rates7.p.rapidapi.com");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "<RAPID_API_KEY>");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "exchange-rates7.p.rapidapi.com");
});

// Gas Price API Client
builder.Services.AddHttpClient("GasPrice", client =>
{
    client.BaseAddress = new Uri("https://gas-price.p.rapidapi.com");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "<RAPID_API_KEY>");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "gas-price.p.rapidapi.com");
});

// OpenWeather API Client
builder.Services.AddHttpClient("OpenWeather", client =>
{
    client.BaseAddress = new Uri("https://open-weather13.p.rapidapi.com");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "<RAPID_API_KEY>");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "open-weather13.p.rapidapi.com");
});
```

---

## 💾 Caching Stratejisi

Uygulama, **In-Memory Cache** kullanarak API çağrılarını optimize eder ve gereksiz istekleri önler:

```csharp
// Servislere ekleme
builder.Services.AddMemoryCache();

// Controller'da kullanım örneği
private readonly IMemoryCache _cache;

public async Task<IActionResult> Index()
{
    // Cache'den veri kontrolü
    if (!_cache.TryGetValue("weather_data", out WeatherInfo cachedWeather))
    {
        // API'den veri çek
        var weatherData = await GetWeatherDataAsync();
        
        // Cache'e kaydet (30 dakika süreyle)
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
        
        _cache.Set("weather_data", weatherData, cacheOptions);
    }
}
```

### Cache Avantajları

| Avantaj | Açıklama |
|---------|----------|
| **Performans** | Tekrarlayan istekler için hızlı yanıt |
| **Maliyet Tasarrufu** | API çağrı limitlerinin korunması |
| **Kullanıcı Deneyimi** | Daha hızlı sayfa yüklenme süreleri |
| **API Rate Limiting** | RapidAPI kota sınırlarına uyum |

---

## 🤖 Google Gemini AI Entegrasyonu

Proje, **Google Gemini AI** kullanarak dinamik içerik üretir:

### Kullanım Alanları

- **🍽️ Günün Yemek Önerisi**: Hava durumuna göre yemek tavsiyesi
- **🗺️ Gezi Rotaları**: Seçilen destinasyona özel rota önerileri

### Request/Response Modelleri

```csharp
// GeminiRequest.cs
public class GeminiRequest
{
    public List<Content> Contents { get; set; }
}

public class Content
{
    public List<Part> Parts { get; set; }
}

public class Part
{
    public string Text { get; set; }
}

// GeminiResponse.cs
public class GeminiResponse
{
    public List<Candidate> Candidates { get; set; }
}

public class Candidate
{
    public ContentResponse Content { get; set; }
}
```

---

## 📊 API Consume (Tüketim) Örnekleri

### 1. Hava Durumu API'si

```csharp
var client = _httpClientFactory.CreateClient("OpenWeather");
var response = await client.GetAsync($"/city/{city}/TR");

if (response.IsSuccessStatusCode)
{
    var json = await response.Content.ReadAsStringAsync();
    var weatherData = JsonSerializer.Deserialize<WeatherInfo>(json);
}
```

### 2. Döviz Kuru API'si

```csharp
var client = _httpClientFactory.CreateClient("ExchangeRate");
var response = await client.GetAsync("/latest?base=USD");

if (response.IsSuccessStatusCode)
{
    var rates = JsonSerializer.Deserialize<ExchangeRateResponse>(json);
    viewModel.USD = rates.Rates["USD"];
    viewModel.EUR = rates.Rates["EUR"];
}
```

### 3. Otel Arama API'si

```csharp
var client = _httpClientFactory.CreateClient("Booking");
var searchResponse = await client.GetAsync($"/api/v1/hotels/searchHotels?dest_id={destId}");

var hotels = JsonSerializer.Deserialize<HotelSearchResponse>(json);
```

---

## 🖼️ Dashboard Özellikleri

### Widget'lar

| Widget | Özellik |
|--------|---------|
| **Döviz Kurları** | USD, EUR, GBP / TRY anlık kurlar |
| **Kripto Paralar** | BTC, ETH, BNB değerleri | ( Coingecko'da olan bir problemden dolayı şuan deaktif )
| **Akaryakıt** | Benzin, Motorin, LPG fiyatları |
| **Hava Durumu** | Sıcaklık, nem, rüzgar bilgileri |
| **Yemek Önerisi** | AI destekli günlük öneri |
| **Gezi Rotaları** | AI destekli 5 farklı rota |
| **Otel Listesi** | Destinasyona göre otel arama |

---

## 🛠️ Kurulum

### Gereksinimler

- .NET 8.0 SDK
- Visual Studio 2022 veya VS Code
- RapidAPI hesabı ve API anahtarları
- Google Gemini API anahtarı

### Adımlar

1. **Repoyu klonlayın:**
```bash
git clone https://github.com/nullablege/AspNetCore-MultiService-Dashboard.git
cd AspNetCore-MultiService-Dashboard
```

2. **API Anahtarlarını yapılandırın:**

`Program.cs` dosyasında `<RAPID_API_KEY>` ve HomeController içerisindeki '<GEMİNİ APİ KEY>' değerlerini kendi API anahtarlarınızla değiştirin.

3. **Uygulamayı çalıştırın:**
```bash
cd Proje7
dotnet restore
dotnet run
```

4. **Tarayıcıda açın:**
```
https://localhost:5001
```

---

## 📚 Kullanılan Teknolojiler

| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| .NET Core | 8.0 | Backend framework |
| ASP.NET Core MVC | 8.0 | Web framework |
| Bootstrap | 5.x | CSS framework |
| jQuery | 3.x | JavaScript library |
| System.Text.Json | - | JSON serialization |
| IMemoryCache | - | In-memory caching |
| IHttpClientFactory | - | HTTP client management |

---

## 🔑 Önemli Kavramlar

### 1. Dependency Injection (DI)
```csharp
public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    
    public HomeController(IHttpClientFactory httpClientFactory, IMemoryCache cache)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
    }
}
```

### 2. Named HttpClient Pattern
```csharp
var client = _httpClientFactory.CreateClient("Booking");
```

### 3. Async/Await Pattern
```csharp
public async Task<IActionResult> Index()
{
    var weatherTask = GetWeatherAsync();
    var exchangeTask = GetExchangeRatesAsync();
    
    await Task.WhenAll(weatherTask, exchangeTask);
}
```

---

## 📸 Ekran Görüntüleri

> Dashboard'un responsive tasarımı sayesinde hem masaüstü hem de mobil cihazlarda sorunsuz çalışır.
<img width="3418" height="1223" alt="Screenshot_120" src="https://github.com/user-attachments/assets/0c8eca70-2c47-4fdd-a6fd-b98db85b0ec1" />
<img width="3439" height="1231" alt="Screenshot_121" src="https://github.com/user-attachments/assets/d8f35804-d4ba-4428-97cf-d2ad44a15e43" />
<img width="3439" height="1227" alt="Screenshot_122" src="https://github.com/user-attachments/assets/7162fe86-257a-4314-a167-05b18ceb6ade" />
<img width="3439" height="1227" alt="Screenshot_123" src="https://github.com/user-attachments/assets/1dd7bf12-df97-4803-adda-913e70ffde8a" />

---

## 📝 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

---

## 👤 Geliştirici

**GitHub:** [@nullablege](https://github.com/nullablege)

---

## 🙏 Teşekkür

Bu proje **M&Y Akademi** Full Stack .Net Bootcamp kapsamında geliştirilmiştir.

---

<div align="center">

**⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın! ⭐**

</div>
