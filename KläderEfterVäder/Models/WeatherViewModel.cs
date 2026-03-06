namespace SmhiVader.Models;

public class WeatherViewModel
{
    // ── Indata från GPS ──────────────────────────────
    public double? Lat { get; set; }
    public double? Lon { get; set; }

    // ── SMHI-data ────────────────────────────────────
    public double? Temperature { get; set; }  // Dagens snitt
    public double? TempMin { get; set; }  // Dagens lägsta
    public double? TempMax { get; set; }  // Dagens högsta
    public double? Precipitation { get; set; }

    // ── Resultat ─────────────────────────────────────
    public ClothingAdvice? Advice { get; set; }
    public string? ErrorMessage { get; set; }

    // ── Hjälpegenskaper för vyn ───────────────────────
    public bool HasResult => Advice != null && Temperature.HasValue;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
}
