using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient("Booking", client =>
{
    client.BaseAddress = new Uri("https://booking-com15.p.rapidapi.com");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "<RAPID APÝ KEY>");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "booking-com15.p.rapidapi.com");

});


builder.Services.AddHttpClient("ExchangeRate", client =>
{
    client.BaseAddress = new Uri("https://exchange-rates7.p.rapidapi.com");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "<RAPID APÝ KEY>");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "exchange-rates7.p.rapidapi.com");

});

builder.Services.AddHttpClient("GasPrice", client =>
{
    client.BaseAddress = new Uri("https://gas-price.p.rapidapi.com");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "<RAPID APÝ KEY>");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "gas-price.p.rapidapi.com");

});

builder.Services.AddHttpClient("OpenWeather", client =>
{
    client.BaseAddress = new Uri("https://open-weather13.p.rapidapi.com");
    client.DefaultRequestHeaders.Add("x-rapidapi-key", "<RAPID APÝ KEY>");
    client.DefaultRequestHeaders.Add("x-rapidapi-host", "open-weather13.p.rapidapi.com");

});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
