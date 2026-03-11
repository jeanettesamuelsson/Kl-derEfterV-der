using Microsoft.AspNetCore.Mvc;
using SmhiVader.Models;
using System.Text.Json;

namespace SmhiVader.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index(double? lat, double? lon)
    {
        var vm = new WeatherViewModel();

        if (lat.HasValue && lon.HasValue)
        {
            vm.Lat = lat.Value;
            vm.Lon = lon.Value;

            
            vm.CityName = await GetCityName(lat.Value, lon.Value);

            try
            {
                var http = _httpClientFactory.CreateClient("smhi");
                var latF = lat.Value.ToString("F6", System.Globalization.CultureInfo.InvariantCulture);
                var lonF = lon.Value.ToString("F6", System.Globalization.CultureInfo.InvariantCulture);
                var url = $"api/category/pmp3g/version/2/geotype/point/lon/{lonF}/lat/{latF}/data.json";

                var resp = await http.GetAsync(url);

                if (!resp.IsSuccessStatusCode)
                {
                    vm.ErrorMessage = $"SMHI svarade med {(int)resp.StatusCode}. Kontrollera att koordinaterna ligger inom Sverige.";
                }
                else
                {
                    var json = await resp.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    var timeSeries = doc.RootElement.GetProperty("timeSeries");

                    var today = DateTime.UtcNow.Date;

                    var todayTemps = new List<double>();
                    var todayPrecips = new List<double>();

                    foreach (var entry in timeSeries.EnumerateArray())
                    {
                        var validTime = entry.GetProperty("validTime").GetString();
                        if (!DateTime.TryParse(validTime, out var dt)) continue;

                        if (dt.Date != today) continue;
                        if (dt.Hour < 6 || dt.Hour > 15) continue;

                        foreach (var p in entry.GetProperty("parameters").EnumerateArray())
                        {
                            var name = p.GetProperty("name").GetString();
                            var val = p.GetProperty("values")[0].GetDouble();
                            if (name == "t") todayTemps.Add(val);
                            if (name == "pmean") todayPrecips.Add(val);
                        }
                    }

                    if (todayTemps.Count > 0)
                    {
                        vm.Temperature = Math.Round(todayTemps.Average(), 1);
                        vm.Precipitation = todayPrecips.Count > 0 ? todayPrecips.Average() : 0;
                        vm.TempMin = todayTemps.Min();
                        vm.TempMax = todayTemps.Max();
                        vm.Advice = ClothingService.GetAdvice(vm.Temperature.Value, vm.Precipitation.Value);
                    }
                    else
                    {
                        vm.ErrorMessage = "Kunde inte hitta prognosdata för dagens datum.";
                    }
                }
            }
            catch (Exception ex)
            {
                vm.ErrorMessage = $"Något gick fel: {ex.Message}";
            }
        }

        return View(vm);
    }

    // ── Reverse geocoding via OpenStreetMap Nominatim ────────────────────
    private async Task<string?> GetCityName(double lat, double lon)
    {
        try
        {
            var http = _httpClientFactory.CreateClient();
            var latF = lat.ToString("F6", System.Globalization.CultureInfo.InvariantCulture);
            var lonF = lon.ToString("F6", System.Globalization.CultureInfo.InvariantCulture);
            var url = $"https://nominatim.openstreetmap.org/reverse?lat={latF}&lon={lonF}&format=json";

            
            http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "ClothesByWeather/1.0");

            var resp = await http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return null;

            var json = await resp.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("address", out var address)) return null;

            
            if (address.TryGetProperty("city", out var city)) return city.GetString();
            if (address.TryGetProperty("municipality", out var mun)) return mun.GetString();
            if (address.TryGetProperty("town", out var town)) return town.GetString();
            if (address.TryGetProperty("village", out var village)) return village.GetString();

            return null;
        }
        catch
        {
            return null; 
        }
    }
}