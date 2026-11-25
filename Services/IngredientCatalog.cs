using BrewBench.Models;

namespace BrewBench.Services;

public class IngredientCatalog
{
    public List<GrainTemplate> Grains { get; } = new()
    {
        new("German Pilsner", 37, 1.7),
        new("Belgian Pilsner", 37, 1.7),
        new("2-Row Pale", 37, 1.8),
        new("Maris Otter", 37, 3.0),
        new("Munich Light (6L)", 35, 6.0),
        new("Munich Dark (10L)", 35, 10.0),
        new("Vienna", 35, 3.5),
        new("Wheat Malt", 37, 2.0),
        new("Flaked Wheat", 36, 1.6),
        new("Flaked Oats", 33, 2.2),
        new("Flaked Barley", 32, 1.5),
        new("Carapils / Dextrin", 33, 1.5),
        new("Caramunich II", 34, 45.0),
        new("Caramunich III", 34, 60.0),
        new("Crystal 40L", 34, 40.0),
        new("Crystal 60L", 34, 60.0),
        new("Crystal 80L", 34, 80.0),
        new("Crystal 120L", 34, 120.0),
        new("Special B", 31, 130.0),
        new("Aromatic", 35, 26.0),
        new("Melanoidin", 33, 27.0),
        new("Biscuit", 36, 25.0),
        new("Victory", 35, 28.0),
        new("Chocolate Malt", 28, 350.0),
        new("Black Patent", 25, 525.0),
        new("Roasted Barley", 25, 450.0),
        new("Carafa Special I", 32, 340.0),
        new("Carafa Special II", 32, 425.0),
        new("Carafa Special III", 32, 525.0),
        new("Acidulated Malt", 27, 2.0),
        new("Rice Hulls", 0, 0.0),
        new("Belgian Candi Sugar (Clear)", 36, 1.0),
        new("Belgian Candi Sugar (Amber)", 36, 35.0),
        new("D2 Dark Candi Syrup", 32, 80.0),
        new("Honey", 35, 1.0),
        new("Table Sugar", 46, 0.0),
        new("Corn Sugar (Dextrose)", 46, 0.5),
    };

    public List<HopTemplate> Hops { get; } = new()
    {
        new("Hallertau Mittelfruh", 4.5, "Noble"),
        new("Hallertau Hersbrucker", 4.0, "Noble"),
        new("Tettnanger", 4.5, "Noble"),
        new("Spalt", 4.5, "Noble"),
        new("Saaz", 3.5, "Noble"),
        new("Styrian Goldings", 5.0, "European"),
        new("East Kent Goldings", 5.0, "English"),
        new("Fuggle", 4.5, "English"),
        new("Northern Brewer", 8.5, "European"),
        new("Magnum", 14.0, "Bittering"),
        new("Cascade", 5.5, "American Aroma"),
        new("Centennial", 10.0, "American Dual"),
        new("Citra", 12.0, "American Aroma"),
        new("Mosaic", 12.5, "American Aroma"),
        new("Simcoe", 13.0, "American Dual"),
        new("Amarillo", 9.0, "American Aroma"),
        new("Columbus", 14.5, "Bittering"),
        new("Chinook", 13.0, "Dual"),
        new("Galaxy", 14.0, "Australian"),
        new("Nelson Sauvin", 12.0, "New Zealand"),
    };

    public List<YeastTemplate> Yeasts { get; } = new()
    {
        new("W-34/70", "Fermentis", 0.83, 48, 58, YeastForm.Dry, "Lager (Weihenstephan)"),
        new("S-189", "Fermentis", 0.82, 48, 58, YeastForm.Dry, "Lager (Hürlimann)"),
        new("S-23", "Fermentis", 0.80, 48, 58, YeastForm.Dry, "Lager"),
        new("WLP830 German Lager", "White Labs", 0.76, 50, 55, YeastForm.Liquid, "Classic German lager"),
        new("WLP833 German Bock", "White Labs", 0.74, 48, 55, YeastForm.Liquid, "Malty bock"),
        new("WLP838 Southern German", "White Labs", 0.74, 50, 55, YeastForm.Liquid, "Helles-friendly"),
        new("WLP400 Belgian Wit II", "White Labs", 0.76, 67, 74, YeastForm.Liquid, "Witbier"),
        new("WLP530 Abbey Ale", "White Labs", 0.78, 66, 72, YeastForm.Liquid, "Belgian Trappist"),
        new("WLP500 Trappist Ale", "White Labs", 0.78, 65, 72, YeastForm.Liquid, "Chimay strain"),
        new("Wyeast 3787 Trappist HG", "Wyeast", 0.76, 64, 78, YeastForm.Liquid, "Westmalle, dubbel/tripel"),
        new("Wyeast 3944 Belgian Wit", "Wyeast", 0.74, 62, 75, YeastForm.Liquid, "Witbier"),
        new("Wyeast 2124 Bohemian Lager", "Wyeast", 0.75, 48, 58, YeastForm.Liquid, "Pilsner"),
        new("Wyeast 2206 Bavarian Lager", "Wyeast", 0.74, 46, 58, YeastForm.Liquid, "Helles/Bock"),
        new("US-05", "Fermentis", 0.81, 59, 75, YeastForm.Dry, "Clean American ale"),
        new("S-04", "Fermentis", 0.75, 59, 75, YeastForm.Dry, "English ale"),
        new("BE-256", "Fermentis", 0.86, 59, 78, YeastForm.Dry, "Belgian dry"),
        new("T-58", "Fermentis", 0.74, 59, 75, YeastForm.Dry, "Belgian/spiced"),
        new("Voss Kveik", "Lallemand", 0.82, 77, 98, YeastForm.Dry, "Norwegian farmhouse"),
    };

    public List<WaterPresetTemplate> WaterProfiles { get; } = new()
    {
        new("RO / Distilled", 0, 0, 0, 0, 0, 0),
        new("Pilsen", 7, 2, 2, 5, 5, 15),
        new("Munich", 75, 18, 10, 10, 2, 152),
        new("Dortmund", 250, 23, 60, 280, 100, 220),
        new("Vienna", 200, 60, 8, 125, 12, 120),
        new("Burton-on-Trent", 270, 60, 50, 640, 35, 200),
        new("Dublin", 117, 4, 12, 55, 19, 200),
        new("London", 70, 6, 15, 40, 38, 166),
        new("Salt Lake City (typical tap)", 60, 15, 32, 50, 40, 150),
    };

    public record GrainTemplate(string Name, double Ppg, double Lovibond);
    public record HopTemplate(string Name, double TypicalAlphaAcid, string Family);
    public record YeastTemplate(string Name, string Lab, double Attenuation, double IdealLowF, double IdealHighF, YeastForm Form, string Description);
    public record WaterPresetTemplate(string Name, double Ca, double Mg, double Na, double SO4, double Cl, double HCO3);
}
