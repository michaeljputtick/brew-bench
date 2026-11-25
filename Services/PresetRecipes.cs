using BrewBench.Models;

namespace BrewBench.Services;

public static class PresetRecipes
{
    public static List<Recipe> All() => new()
    {
        GermanPilsner(),
        MunichHelles(),
        Witbier(),
        ViennaLager(),
        Dubbel(),
        Tripel(),
        Doppelbock(),
    };

    private static Recipe Common(string id, string name, string styleId, string notes) => new()
    {
        Id = "preset-" + id,
        Name = name,
        StyleId = styleId,
        Notes = notes,
        IsPreset = true,
        BatchSizeGallons = 5.5,
        BoilOffGalPerHour = 1.0,
        TrubLossGallons = 0.5,
        MashEfficiency = 0.78,
        MashThicknessQtPerLb = 1.4,
        GrainTempF = 70,
    };

    private static Recipe GermanPilsner()
    {
        var r = Common("german-pilsner", "German Pilsner", "5D", "Crisp, dry, noble-hopped. 90-min boil for DMS reduction. Long lager required.");
        r.BoilMinutes = 90;
        r.MashSchedule = new() { new MashStep { Name = "Saccharification", TempF = 148, Minutes = 60 } };
        r.Grains.Add(new() { Name = "German Pilsner", Pounds = 9.25, Ppg = 37, Lovibond = 1.7 });
        r.Grains.Add(new() { Name = "Carapils / Dextrin", Pounds = 0.5, Ppg = 33, Lovibond = 1.5 });
        r.Hops.Add(new() { Name = "Hallertau Mittelfruh", Ounces = 1.5, AlphaAcid = 4.5, BoilMinutes = 60 });
        r.Hops.Add(new() { Name = "Hallertau Mittelfruh", Ounces = 1.0, AlphaAcid = 4.5, BoilMinutes = 20 });
        r.Hops.Add(new() { Name = "Hallertau Mittelfruh", Ounces = 1.0, AlphaAcid = 4.5, BoilMinutes = 5 });
        r.Yeasts.Add(new() { Name = "W-34/70", Lab = "Fermentis", ApparentAttenuation = 0.80, IdealLowF = 50, IdealHighF = 52, Form = YeastForm.Dry });
        r.SourceWater = RoWater();
        r.TargetWater = PilsenWater();
        return r;
    }

    private static Recipe MunichHelles()
    {
        var r = Common("munich-helles", "Munich Helles", "4A", "Soft, malt-forward pale lager. 90-min boil.");
        r.BoilMinutes = 90;
        r.MashSchedule = new() { new MashStep { Name = "Saccharification", TempF = 152, Minutes = 60 } };
        r.Grains.Add(new() { Name = "German Pilsner", Pounds = 7.5, Ppg = 37, Lovibond = 1.7 });
        r.Grains.Add(new() { Name = "Munich Light (6L)", Pounds = 2.5, Ppg = 35, Lovibond = 6.0 });
        r.Hops.Add(new() { Name = "Hallertau Mittelfruh", Ounces = 1.25, AlphaAcid = 4.5, BoilMinutes = 60 });
        r.Hops.Add(new() { Name = "Hallertau Mittelfruh", Ounces = 0.5, AlphaAcid = 4.5, BoilMinutes = 15 });
        r.Yeasts.Add(new() { Name = "W-34/70", Lab = "Fermentis", ApparentAttenuation = 0.80, IdealLowF = 50, IdealHighF = 52, Form = YeastForm.Dry });
        r.SourceWater = RoWater();
        r.TargetWater = MunichWater();
        return r;
    }

    private static Recipe Witbier()
    {
        var r = Common("witbier", "Belgian Witbier", "24A", "Step mash recommended. Don't filter; cloudy is correct.");
        r.BoilMinutes = 60;
        r.MashEfficiency = 0.72;
        r.MashSchedule = new()
        {
            new MashStep { Name = "Protein rest", TempF = 122, Minutes = 30 },
            new MashStep { Name = "Saccharification", TempF = 152, Minutes = 60 },
            new MashStep { Name = "Mashout", TempF = 168, Minutes = 10 },
        };
        r.Grains.Add(new() { Name = "Belgian Pilsner", Pounds = 6.5, Ppg = 37, Lovibond = 1.7 });
        r.Grains.Add(new() { Name = "Flaked Wheat", Pounds = 4.0, Ppg = 36, Lovibond = 1.6 });
        r.Grains.Add(new() { Name = "Flaked Oats", Pounds = 0.5, Ppg = 33, Lovibond = 2.2 });
        r.Grains.Add(new() { Name = "Rice Hulls", Pounds = 1.0, Ppg = 0, Lovibond = 0 });
        r.Hops.Add(new() { Name = "Saaz", Ounces = 1.0, AlphaAcid = 3.5, BoilMinutes = 60 });
        r.Hops.Add(new() { Name = "Saaz", Ounces = 0.5, AlphaAcid = 3.5, BoilMinutes = 10 });
        r.Yeasts.Add(new() { Name = "WLP400 Belgian Wit II", Lab = "White Labs", ApparentAttenuation = 0.76, IdealLowF = 67, IdealHighF = 72, Form = YeastForm.Liquid });
        r.SourceWater = RoWater();
        r.TargetWater = new WaterProfile { Name = "Witbier Target", Calcium = 50, Magnesium = 5, Sodium = 10, Sulfate = 50, Chloride = 50, Bicarbonate = 50 };
        return r;
    }

    private static Recipe ViennaLager()
    {
        var r = Common("vienna-lager", "Vienna Lager", "7A", "Toasty, malt-forward amber lager. Optional decoction step.");
        r.BoilMinutes = 60;
        r.MashSchedule = new() { new MashStep { Name = "Saccharification", TempF = 152, Minutes = 60 } };
        r.Grains.Add(new() { Name = "Vienna", Pounds = 6.0, Ppg = 35, Lovibond = 3.5 });
        r.Grains.Add(new() { Name = "Munich Light (6L)", Pounds = 4.0, Ppg = 35, Lovibond = 6.0 });
        r.Grains.Add(new() { Name = "Caramunich II", Pounds = 1.0, Ppg = 34, Lovibond = 45.0 });
        r.Grains.Add(new() { Name = "Melanoidin", Pounds = 0.5, Ppg = 33, Lovibond = 27.0 });
        r.Hops.Add(new() { Name = "Hallertau Mittelfruh", Ounces = 1.0, AlphaAcid = 4.5, BoilMinutes = 60 });
        r.Hops.Add(new() { Name = "Hallertau Mittelfruh", Ounces = 0.75, AlphaAcid = 4.5, BoilMinutes = 20 });
        r.Hops.Add(new() { Name = "Hallertau Mittelfruh", Ounces = 0.5, AlphaAcid = 4.5, BoilMinutes = 5 });
        r.Yeasts.Add(new() { Name = "W-34/70", Lab = "Fermentis", ApparentAttenuation = 0.78, IdealLowF = 50, IdealHighF = 54, Form = YeastForm.Dry });
        r.SourceWater = RoWater();
        r.TargetWater = ViennaWater();
        return r;
    }

    private static Recipe Dubbel()
    {
        var r = Common("dubbel", "Belgian Dubbel", "26B", "D2 candi syrup is non-negotiable. Ramp ferment to 74F.");
        r.BoilMinutes = 60;
        r.MashSchedule = new() { new MashStep { Name = "Saccharification", TempF = 152, Minutes = 60 } };
        r.Grains.Add(new() { Name = "Belgian Pilsner", Pounds = 11.0, Ppg = 37, Lovibond = 1.7 });
        r.Grains.Add(new() { Name = "Special B", Pounds = 0.5, Ppg = 31, Lovibond = 130.0 });
        r.Grains.Add(new() { Name = "Caramunich III", Pounds = 0.5, Ppg = 34, Lovibond = 60.0 });
        r.Grains.Add(new() { Name = "Aromatic", Pounds = 0.5, Ppg = 35, Lovibond = 26.0 });
        r.Grains.Add(new() { Name = "D2 Dark Candi Syrup", Pounds = 1.0, Ppg = 32, Lovibond = 80.0, Use = GrainUse.LateBoil });
        r.Hops.Add(new() { Name = "Styrian Goldings", Ounces = 1.0, AlphaAcid = 5.0, BoilMinutes = 60 });
        r.Hops.Add(new() { Name = "Styrian Goldings", Ounces = 0.5, AlphaAcid = 5.0, BoilMinutes = 20 });
        r.Hops.Add(new() { Name = "Styrian Goldings", Ounces = 0.25, AlphaAcid = 5.0, BoilMinutes = 5 });
        r.Yeasts.Add(new() { Name = "Wyeast 3787 Trappist HG", Lab = "Wyeast", ApparentAttenuation = 0.78, IdealLowF = 65, IdealHighF = 74, Form = YeastForm.Liquid });
        r.SourceWater = RoWater();
        r.TargetWater = new WaterProfile { Name = "Belgian Target", Calcium = 80, Magnesium = 5, Sodium = 25, Sulfate = 60, Chloride = 60, Bicarbonate = 100 };
        return r;
    }

    private static Recipe Tripel()
    {
        var r = Common("tripel", "Belgian Tripel", "26C", "Low mash for fermentability. 2L starter mandatory. Don't exceed 80F.");
        r.BoilMinutes = 75;
        r.MashSchedule = new() { new MashStep { Name = "Saccharification", TempF = 148, Minutes = 75 } };
        r.Grains.Add(new() { Name = "Belgian Pilsner", Pounds = 12.5, Ppg = 37, Lovibond = 1.7 });
        r.Grains.Add(new() { Name = "Munich Light (6L)", Pounds = 1.5, Ppg = 35, Lovibond = 6.0 });
        r.Grains.Add(new() { Name = "Belgian Candi Sugar (Clear)", Pounds = 2.0, Ppg = 36, Lovibond = 1.0, Use = GrainUse.Flameout });
        r.Hops.Add(new() { Name = "Styrian Goldings", Ounces = 2.0, AlphaAcid = 5.0, BoilMinutes = 60 });
        r.Hops.Add(new() { Name = "Styrian Goldings", Ounces = 0.5, AlphaAcid = 5.0, BoilMinutes = 30 });
        r.Hops.Add(new() { Name = "Saaz", Ounces = 0.75, AlphaAcid = 3.5, BoilMinutes = 15 });
        r.Hops.Add(new() { Name = "Saaz", Ounces = 0.5, AlphaAcid = 3.5, BoilMinutes = 5 });
        r.Yeasts.Add(new() { Name = "Wyeast 3787 Trappist HG", Lab = "Wyeast", ApparentAttenuation = 0.78, IdealLowF = 65, IdealHighF = 75, Form = YeastForm.Liquid });
        r.SourceWater = RoWater();
        r.TargetWater = new WaterProfile { Name = "Belgian Tripel", Calcium = 60, Magnesium = 5, Sodium = 10, Sulfate = 60, Chloride = 40, Bicarbonate = 50 };
        return r;
    }

    private static Recipe Doppelbock()
    {
        var r = Common("doppelbock", "Doppelbock", "9A", "Rich winter lager. 90-min boil. Lager at least 8 weeks.");
        r.BoilMinutes = 90;
        r.MashSchedule = new() { new MashStep { Name = "Saccharification", TempF = 152, Minutes = 75 } };
        r.Grains.Add(new() { Name = "Munich Dark (10L)", Pounds = 10.0, Ppg = 35, Lovibond = 10.0 });
        r.Grains.Add(new() { Name = "Munich Light (6L)", Pounds = 5.0, Ppg = 35, Lovibond = 6.0 });
        r.Grains.Add(new() { Name = "German Pilsner", Pounds = 1.0, Ppg = 37, Lovibond = 1.7 });
        r.Grains.Add(new() { Name = "Caramunich III", Pounds = 1.0, Ppg = 34, Lovibond = 60.0 });
        r.Grains.Add(new() { Name = "Melanoidin", Pounds = 0.5, Ppg = 33, Lovibond = 27.0 });
        r.Grains.Add(new() { Name = "Carafa Special II", Pounds = 0.25, Ppg = 32, Lovibond = 425.0 });
        r.Hops.Add(new() { Name = "Hallertau Mittelfruh", Ounces = 1.5, AlphaAcid = 4.5, BoilMinutes = 60 });
        r.Hops.Add(new() { Name = "Hallertau Mittelfruh", Ounces = 0.75, AlphaAcid = 4.5, BoilMinutes = 30 });
        r.Yeasts.Add(new() { Name = "WLP833 German Bock", Lab = "White Labs", ApparentAttenuation = 0.74, IdealLowF = 48, IdealHighF = 52, Form = YeastForm.Liquid });
        r.SourceWater = RoWater();
        r.TargetWater = MunichWater();
        return r;
    }

    // Presets build from RO so salt additions are driven by the target; swap in your own tap profile on the Water page.
    private static WaterProfile RoWater() => new() { Name = "RO / Distilled" };

    private static WaterProfile PilsenWater() => new()
    { Name = "Pilsen", Calcium = 7, Magnesium = 2, Sodium = 2, Sulfate = 5, Chloride = 5, Bicarbonate = 15 };

    private static WaterProfile MunichWater() => new()
    { Name = "Munich", Calcium = 75, Magnesium = 18, Sodium = 10, Sulfate = 10, Chloride = 2, Bicarbonate = 152 };

    private static WaterProfile ViennaWater() => new()
    { Name = "Vienna", Calcium = 200, Magnesium = 60, Sodium = 8, Sulfate = 125, Chloride = 12, Bicarbonate = 120 };
}
