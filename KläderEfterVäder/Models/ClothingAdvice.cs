namespace SmhiVader.Models;

// ── Modell för ett klädråd ───────────────────────────────────────────────────
public class ClothingAdvice
{
    public string Title { get; init; } = "";
    public string Icon { get; init; } = "";
    public string Description { get; init; } = "";
    public string BgColor { get; init; } = "#37474f";
    public string AccentColor { get; init; } = "#90a4ae";
}

// ── All klädlogik samlad på ett ställe ──────────────────────────────────────
public static class ClothingService
{
    public static ClothingAdvice GetAdvice(double temp, double rain)
    {
        bool isRainy = rain > 0.1;

        return temp switch
        {
            <= -15 => new ClothingAdvice
            {
                Title = "Vinteroverall+",
                Icon = "❄️",
                Description = "Vinteroverall, tjocktröja/fleece eller underställ, mössa, halsduk, vantar & vinterkängor. ",
                BgColor = "#0d1b4a",
                AccentColor = "#4fc3f7"
            },
            <= -5 => new ClothingAdvice
            {
                Title = "Vinteroverall",
                Icon = "🧥",
                Description = "Vinteroverall, mössa, vantar och vinterkängor / fodrade stövlar.",
                BgColor = "#1a237e",
                AccentColor = "#90caf9"
            },
            <= 0 => new ClothingAdvice
            {
                Title = "Vinteroverall",
                Icon = "🧣",
                Description = "Vinteroverall, mössa, vantar och vinterkängor / fodrade stövlar.",
                BgColor = "#283593",
                AccentColor = "#b3e5fc"
            },
            <= 5 => new ClothingAdvice
            {
                Title = "Skal + fleece ",
                Icon = "🧤",
                Description = "Skaljacka/skaloverall, fodrad mössa, fleece eller fodrat regnställ.",
                BgColor = "#37474f",
                AccentColor = "#80deea"
            },
            <= 10 => new ClothingAdvice
            {
                Title = "Skal + fleece ",
                Icon = "🧦",
                Description = "Skaljacka/skaloverall, fleece, tunn mössa" + (isRainy ? " Regn idag - Fodrat regnställ + gummistövlar!" : " "),
                BgColor = "#004d40",
                AccentColor = "#80cbc4"
            },
            <= 15 => new ClothingAdvice
            {
                Title = "Skaljacka eller vårjacka",
                Icon = "🍃",
                Description = "Tunnare jacka eller skalkläder." + (isRainy ? " Ha med regnkläder!" : " Lite svalt i skuggan."),
                BgColor = "#1b5e20",
                AccentColor = "#a5d6a7"
            },
            <= 20 => new ClothingAdvice
            {
                Title = "Tunn tröja",
                Icon = "☀️",
                Description = "Vindfleece, ev väst eller skaljacka." + (isRainy ? " Regnjacka och gummistövlar." : " Solglasögon kan vara bra!"),
                BgColor = "#e65100",
                AccentColor = "#ffcc80"
            },
            <= 25 => new ClothingAdvice
            {
                Title = "T-shirt & shorts",
                Icon = "🩳",
                Description = "T-shirt och shorts. Kom ihåg solskyddsmedel och hatt!" + (isRainy ? " Regnjacka och gummistövlar." : ""),
                BgColor = "#bf360c",
                AccentColor = "#ffab91"
            },
            _ => new ClothingAdvice
            {
                Title = "Sommarkläder",
                Icon = "🌞",
                Description = "Lätta kläder, solhatt och solskyddsmedel. Drick mycket vatten och håll dig i skuggan!",
                BgColor = "#4e342e",
                AccentColor = "#ffccbc"
            }

        };


    }
}