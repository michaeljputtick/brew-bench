using BrewBench.Models;

namespace BrewBench.Services;

public class StyleCatalog
{
    public List<BjcpStyle> Styles { get; } = new()
    {
        new() { Id = "1A", Number = "1A", Name = "American Light Lager", Category = "Light Lager",
                Og = new() { Min = 1.028, Max = 1.040 }, Fg = new() { Min = 0.998, Max = 1.008 },
                Abv = new() { Min = 2.8, Max = 4.2 }, Ibu = new() { Min = 8, Max = 12 }, Srm = new() { Min = 2, Max = 3 } },
        new() { Id = "1D", Number = "1D", Name = "American Lager", Category = "Light Lager",
                Og = new() { Min = 1.040, Max = 1.050 }, Fg = new() { Min = 1.004, Max = 1.010 },
                Abv = new() { Min = 4.2, Max = 5.3 }, Ibu = new() { Min = 8, Max = 18 }, Srm = new() { Min = 2, Max = 4 } },
        new() { Id = "2A", Number = "2A", Name = "International Pale Lager", Category = "Pale Lager",
                Og = new() { Min = 1.042, Max = 1.050 }, Fg = new() { Min = 1.008, Max = 1.012 },
                Abv = new() { Min = 4.6, Max = 6.0 }, Ibu = new() { Min = 18, Max = 25 }, Srm = new() { Min = 2, Max = 6 } },
        new() { Id = "3A", Number = "3A", Name = "Czech Pale Lager", Category = "Czech Lager",
                Og = new() { Min = 1.028, Max = 1.044 }, Fg = new() { Min = 1.008, Max = 1.014 },
                Abv = new() { Min = 3.0, Max = 4.1 }, Ibu = new() { Min = 20, Max = 35 }, Srm = new() { Min = 3, Max = 6 } },
        new() { Id = "3B", Number = "3B", Name = "Czech Premium Pale Lager", Category = "Czech Lager",
                Og = new() { Min = 1.044, Max = 1.060 }, Fg = new() { Min = 1.013, Max = 1.017 },
                Abv = new() { Min = 4.2, Max = 5.8 }, Ibu = new() { Min = 30, Max = 45 }, Srm = new() { Min = 3.5, Max = 6 } },
        new() { Id = "4A", Number = "4A", Name = "Munich Helles", Category = "Pale Bitter Lager",
                Og = new() { Min = 1.044, Max = 1.048 }, Fg = new() { Min = 1.006, Max = 1.012 },
                Abv = new() { Min = 4.7, Max = 5.4 }, Ibu = new() { Min = 16, Max = 22 }, Srm = new() { Min = 3, Max = 5 } },
        new() { Id = "4B", Number = "4B", Name = "Festbier", Category = "Pale Bitter Lager",
                Og = new() { Min = 1.054, Max = 1.057 }, Fg = new() { Min = 1.010, Max = 1.012 },
                Abv = new() { Min = 5.8, Max = 6.3 }, Ibu = new() { Min = 18, Max = 25 }, Srm = new() { Min = 4, Max = 7 } },
        new() { Id = "5D", Number = "5D", Name = "German Pils", Category = "Pale Bitter Lager",
                Og = new() { Min = 1.044, Max = 1.050 }, Fg = new() { Min = 1.008, Max = 1.013 },
                Abv = new() { Min = 4.4, Max = 5.2 }, Ibu = new() { Min = 22, Max = 40 }, Srm = new() { Min = 2, Max = 5 } },
        new() { Id = "6A", Number = "6A", Name = "Märzen", Category = "Amber Malty Lager",
                Og = new() { Min = 1.054, Max = 1.060 }, Fg = new() { Min = 1.010, Max = 1.014 },
                Abv = new() { Min = 5.8, Max = 6.3 }, Ibu = new() { Min = 18, Max = 24 }, Srm = new() { Min = 8, Max = 17 } },
        new() { Id = "7A", Number = "7A", Name = "Vienna Lager", Category = "Amber Bitter Lager",
                Og = new() { Min = 1.048, Max = 1.055 }, Fg = new() { Min = 1.010, Max = 1.014 },
                Abv = new() { Min = 4.7, Max = 5.5 }, Ibu = new() { Min = 18, Max = 30 }, Srm = new() { Min = 9, Max = 15 } },
        new() { Id = "8A", Number = "8A", Name = "Munich Dunkel", Category = "Dark Malty Lager",
                Og = new() { Min = 1.048, Max = 1.056 }, Fg = new() { Min = 1.010, Max = 1.016 },
                Abv = new() { Min = 4.5, Max = 5.6 }, Ibu = new() { Min = 18, Max = 28 }, Srm = new() { Min = 14, Max = 28 } },
        new() { Id = "9A", Number = "9A", Name = "Doppelbock", Category = "Strong European Lager",
                Og = new() { Min = 1.072, Max = 1.112 }, Fg = new() { Min = 1.016, Max = 1.024 },
                Abv = new() { Min = 7.0, Max = 10.0 }, Ibu = new() { Min = 16, Max = 26 }, Srm = new() { Min = 6, Max = 25 } },
        new() { Id = "9B", Number = "9B", Name = "Eisbock", Category = "Strong European Lager",
                Og = new() { Min = 1.078, Max = 1.120 }, Fg = new() { Min = 1.020, Max = 1.035 },
                Abv = new() { Min = 9.0, Max = 14.0 }, Ibu = new() { Min = 25, Max = 35 }, Srm = new() { Min = 18, Max = 30 } },
        new() { Id = "10A", Number = "10A", Name = "Weissbier", Category = "German Wheat",
                Og = new() { Min = 1.044, Max = 1.052 }, Fg = new() { Min = 1.010, Max = 1.014 },
                Abv = new() { Min = 4.3, Max = 5.6 }, Ibu = new() { Min = 8, Max = 15 }, Srm = new() { Min = 2, Max = 6 } },
        new() { Id = "10B", Number = "10B", Name = "Dunkles Weissbier", Category = "German Wheat",
                Og = new() { Min = 1.044, Max = 1.056 }, Fg = new() { Min = 1.010, Max = 1.014 },
                Abv = new() { Min = 4.3, Max = 5.6 }, Ibu = new() { Min = 10, Max = 18 }, Srm = new() { Min = 14, Max = 23 } },
        new() { Id = "12A", Number = "12A", Name = "British Golden Ale", Category = "Pale Commonwealth",
                Og = new() { Min = 1.038, Max = 1.053 }, Fg = new() { Min = 1.006, Max = 1.012 },
                Abv = new() { Min = 3.8, Max = 5.0 }, Ibu = new() { Min = 20, Max = 45 }, Srm = new() { Min = 2, Max = 6 } },
        new() { Id = "13C", Number = "13C", Name = "English Porter", Category = "Brown British Beer",
                Og = new() { Min = 1.040, Max = 1.052 }, Fg = new() { Min = 1.008, Max = 1.014 },
                Abv = new() { Min = 4.0, Max = 5.4 }, Ibu = new() { Min = 18, Max = 35 }, Srm = new() { Min = 20, Max = 30 } },
        new() { Id = "14A", Number = "14A", Name = "Scottish Light", Category = "Scottish Ale",
                Og = new() { Min = 1.030, Max = 1.035 }, Fg = new() { Min = 1.010, Max = 1.013 },
                Abv = new() { Min = 2.5, Max = 3.2 }, Ibu = new() { Min = 10, Max = 20 }, Srm = new() { Min = 17, Max = 22 } },
        new() { Id = "15A", Number = "15A", Name = "Irish Red Ale", Category = "Irish",
                Og = new() { Min = 1.036, Max = 1.046 }, Fg = new() { Min = 1.010, Max = 1.014 },
                Abv = new() { Min = 3.8, Max = 5.0 }, Ibu = new() { Min = 18, Max = 28 }, Srm = new() { Min = 9, Max = 18 } },
        new() { Id = "15B", Number = "15B", Name = "Irish Stout", Category = "Irish",
                Og = new() { Min = 1.036, Max = 1.044 }, Fg = new() { Min = 1.007, Max = 1.011 },
                Abv = new() { Min = 4.0, Max = 4.5 }, Ibu = new() { Min = 25, Max = 45 }, Srm = new() { Min = 25, Max = 40 } },
        new() { Id = "20A", Number = "20A", Name = "American Porter", Category = "American Porter & Stout",
                Og = new() { Min = 1.050, Max = 1.070 }, Fg = new() { Min = 1.012, Max = 1.018 },
                Abv = new() { Min = 4.8, Max = 6.5 }, Ibu = new() { Min = 25, Max = 50 }, Srm = new() { Min = 22, Max = 40 } },
        new() { Id = "21A", Number = "21A", Name = "American IPA", Category = "IPA",
                Og = new() { Min = 1.056, Max = 1.070 }, Fg = new() { Min = 1.008, Max = 1.014 },
                Abv = new() { Min = 5.5, Max = 7.5 }, Ibu = new() { Min = 40, Max = 70 }, Srm = new() { Min = 6, Max = 14 } },
        new() { Id = "21B", Number = "21B", Name = "New England IPA", Category = "IPA",
                Og = new() { Min = 1.060, Max = 1.085 }, Fg = new() { Min = 1.010, Max = 1.015 },
                Abv = new() { Min = 6.0, Max = 9.0 }, Ibu = new() { Min = 25, Max = 60 }, Srm = new() { Min = 3, Max = 7 } },
        new() { Id = "24A", Number = "24A", Name = "Witbier", Category = "Belgian Ale",
                Og = new() { Min = 1.044, Max = 1.052 }, Fg = new() { Min = 1.008, Max = 1.012 },
                Abv = new() { Min = 4.5, Max = 5.5 }, Ibu = new() { Min = 8, Max = 20 }, Srm = new() { Min = 2, Max = 4 } },
        new() { Id = "25B", Number = "25B", Name = "Saison", Category = "Strong Belgian Ale",
                Og = new() { Min = 1.048, Max = 1.065 }, Fg = new() { Min = 1.002, Max = 1.008 },
                Abv = new() { Min = 5.0, Max = 7.0 }, Ibu = new() { Min = 20, Max = 35 }, Srm = new() { Min = 5, Max = 14 } },
        new() { Id = "26A", Number = "26A", Name = "Belgian Single (Trappist Single)", Category = "Trappist Ale",
                Og = new() { Min = 1.044, Max = 1.054 }, Fg = new() { Min = 1.004, Max = 1.010 },
                Abv = new() { Min = 4.8, Max = 6.0 }, Ibu = new() { Min = 25, Max = 45 }, Srm = new() { Min = 3, Max = 5 } },
        new() { Id = "26B", Number = "26B", Name = "Belgian Dubbel", Category = "Trappist Ale",
                Og = new() { Min = 1.062, Max = 1.075 }, Fg = new() { Min = 1.008, Max = 1.018 },
                Abv = new() { Min = 6.0, Max = 7.6 }, Ibu = new() { Min = 15, Max = 25 }, Srm = new() { Min = 10, Max = 17 } },
        new() { Id = "26C", Number = "26C", Name = "Belgian Tripel", Category = "Trappist Ale",
                Og = new() { Min = 1.075, Max = 1.085 }, Fg = new() { Min = 1.008, Max = 1.014 },
                Abv = new() { Min = 7.5, Max = 9.5 }, Ibu = new() { Min = 20, Max = 40 }, Srm = new() { Min = 4.5, Max = 7 } },
        new() { Id = "26D", Number = "26D", Name = "Belgian Dark Strong Ale (Quad)", Category = "Trappist Ale",
                Og = new() { Min = 1.075, Max = 1.110 }, Fg = new() { Min = 1.010, Max = 1.024 },
                Abv = new() { Min = 8.0, Max = 12.0 }, Ibu = new() { Min = 20, Max = 35 }, Srm = new() { Min = 12, Max = 22 } },
    };

    public BjcpStyle? Find(string id) => Styles.FirstOrDefault(s => s.Id == id);

    public List<StyleCheck> Check(BjcpStyle? style, CalcResult result)
    {
        var checks = new List<StyleCheck>();
        if (style is null) return checks;
        checks.Add(Verdict("OG", result.Og, style.Og.Min, style.Og.Max, "F4"));
        checks.Add(Verdict("FG", result.Fg, style.Fg.Min, style.Fg.Max, "F4"));
        checks.Add(Verdict("ABV %", result.Abv, style.Abv.Min, style.Abv.Max, "F1"));
        checks.Add(Verdict("IBU", result.Ibu, style.Ibu.Min, style.Ibu.Max, "F0"));
        checks.Add(Verdict("SRM", result.Srm, style.Srm.Min, style.Srm.Max, "F1"));
        return checks;
    }

    private static StyleCheck Verdict(string label, double value, double min, double max, string fmt)
    {
        Severity s;
        if (value >= min && value <= max) s = Severity.Ok;
        else
        {
            double span = max - min;
            double dist = value < min ? min - value : value - max;
            s = dist <= span * 0.10 ? Severity.Warning : Severity.Bad;
        }
        return new StyleCheck(label, value, min, max, s, fmt);
    }
}

public enum Severity { Ok, Warning, Bad }
public record StyleCheck(string Label, double Value, double Min, double Max, Severity Severity, string Format);
