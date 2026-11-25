using System.Text.Json.Serialization;

namespace BrewBench.Models;

public class Recipe
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "New Recipe";
    public string StyleId { get; set; } = "";
    public string Notes { get; set; } = "";

    // Batch parameters
    public double BatchSizeGallons { get; set; } = 5.5;
    public double BoilMinutes { get; set; } = 60;
    public double BoilOffGalPerHour { get; set; } = 1.0;
    public double TrubLossGallons { get; set; } = 0.5;
    public double MashEfficiency { get; set; } = 0.78;

    // Mash
    public List<MashStep> MashSchedule { get; set; } = new();
    public double GrainTempF { get; set; } = 70;
    public double MashThicknessQtPerLb { get; set; } = 1.4;

    // Ingredients
    public List<GrainAddition> Grains { get; set; } = new();
    public List<HopAddition> Hops { get; set; } = new();
    public List<YeastAddition> Yeasts { get; set; } = new();

    // Water
    public WaterProfile? SourceWater { get; set; }
    public WaterProfile? TargetWater { get; set; }

    // Lock and solve
    public string? LockedTarget { get; set; }       // "ABV" | "OG" | "IBU" | null
    public double? LockedTargetValue { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

    public bool IsPreset { get; set; }

    public Recipe Clone()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(this);
        var copy = System.Text.Json.JsonSerializer.Deserialize<Recipe>(json)!;
        copy.Id = Guid.NewGuid().ToString("N");
        copy.IsPreset = false;
        copy.CreatedUtc = DateTime.UtcNow;
        copy.UpdatedUtc = DateTime.UtcNow;
        return copy;
    }
}

public class MashStep
{
    public string Name { get; set; } = "Saccharification";
    public double TempF { get; set; } = 152;
    public int Minutes { get; set; } = 60;
}

public class GrainAddition
{
    public string Name { get; set; } = "";
    public double Pounds { get; set; }
    public double Ppg { get; set; } = 36;
    public double Lovibond { get; set; } = 2;
    public GrainUse Use { get; set; } = GrainUse.Mash;
}

public enum GrainUse { Mash, LateBoil, Flameout }

public class HopAddition
{
    public string Name { get; set; } = "";
    public double Ounces { get; set; }
    public double AlphaAcid { get; set; } = 4.5;
    public int BoilMinutes { get; set; } = 60;
    public HopUse Use { get; set; } = HopUse.Boil;
}

public enum HopUse { Boil, Whirlpool, DryHop, FirstWort }

public class YeastAddition
{
    public string Name { get; set; } = "";
    public string Lab { get; set; } = "";
    public double ApparentAttenuation { get; set; } = 0.78;
    public double IdealLowF { get; set; } = 50;
    public double IdealHighF { get; set; } = 55;
    public YeastForm Form { get; set; } = YeastForm.Dry;
}

public enum YeastForm { Liquid, Dry }

public class WaterProfile
{
    public string Name { get; set; } = "Custom";
    public double Calcium { get; set; }      // ppm
    public double Magnesium { get; set; }
    public double Sodium { get; set; }
    public double Sulfate { get; set; }
    public double Chloride { get; set; }
    public double Bicarbonate { get; set; }
}
